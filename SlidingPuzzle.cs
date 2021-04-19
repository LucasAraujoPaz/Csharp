/**
LeetCode's Hard challenge Sliding Puzzle

Site: https://leetcode.com/problems/sliding-puzzle/

Challenge:

On a 2x3 board, there are 5 tiles represented by the integers 1 through 5, and an empty square represented by 0.

A move consists of choosing 0 and a 4-directionally adjacent number and swapping it.

The state of the board is solved if and only if the board is [[1,2,3],[4,5,0]].

Given a puzzle board, return the least number of moves required so that the state of the board is solved.
If it is impossible for the state of the board to be solved, return -1.

Example:
Input: board = [[1,2,3],[4,0,5]]
Output: 1
Explanation: Swap the 0 and the 5 in one move.

------------------------------------------------

My solution uses breadth-first through queues and a kind of "decimal mask", represented by the variable "stateToInt".
It beats 100% other C# solutions in runtime.

Author of Solution: Lucas Paz.
*/

public class Solution 
{
    private static readonly int SOLVED = 123450;
    
    private Queue<int[][]> states;
    private Queue<int> numberOfMoves;
    
    public int SlidingPuzzle(int[][] board)
    {
        int minimumNumberOfMoves = -1;
        
        states = new Queue<int[][]>();
        numberOfMoves = new Queue<int>();
        
        ISet<int> visitedStates = new HashSet<int>();
        
        states.Enqueue(board);
        numberOfMoves.Enqueue(0);
        
        while (states.Count > 0)
        {
            int[][] state = states.Dequeue();
            int move = numberOfMoves.Dequeue();
            
            int rowOf0=-1, columnOf0=-1;
            int stateToInt = 0;
            
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 3; j++)
                {
                    int tile = state[i][j];
                    
                    if (tile == 0) (rowOf0, columnOf0) = (i, j);
                    
                    stateToInt = (stateToInt * 10) + tile;   
                }
            
            if (stateToInt == SOLVED) return move;
            
            if (visitedStates.Contains(stateToInt))
                continue;
            
            MoveIn4Directions(state, rowOf0, columnOf0, move);      
            
            visitedStates.Add(stateToInt);
        }
        
        return minimumNumberOfMoves;
    }
    
    private void MoveIn4Directions(int[][] state, int rowOf0, int columnOf0, int move)
    {
        if (rowOf0 == 0)
            Swap(state, rowOf0, columnOf0, 1, columnOf0, move);
        
        if (rowOf0 == 1)
            Swap(state, rowOf0, columnOf0, 0, columnOf0, move);
        
        if (columnOf0 < 2)
            Swap(state, rowOf0, columnOf0, rowOf0, columnOf0 + 1, move);
        
        if (columnOf0 > 0)
            Swap(state, rowOf0, columnOf0, rowOf0, columnOf0 - 1, move);
    }
    
    private void Swap(int[][] state, int rowOf0, int columnOf0, int rowOfOther, int columnOfOther, int move)
    {
        int[][] clone = new int[2][] {
            new int[3] {state[0][0], state[0][1], state[0][2]},
            new int[3] {state[1][0], state[1][1], state[1][2]}
        };       
        
        clone[rowOfOther][columnOfOther] = 0;
        clone[rowOf0][columnOf0] = state[rowOfOther][columnOfOther];
        
        states.Enqueue(clone);
        numberOfMoves.Enqueue(move + 1);
    }
}
