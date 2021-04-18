/**
My solution to LeetCode's hard challenge "Minimum Distance to Type a Word Using Two Fingers".

Site of the challenge: https://leetcode.com/problems/minimum-distance-to-type-a-word-using-two-fingers/

Challenge:

------------
A B C D E F 
G H I J K L 
M N O P Q R 
S T U V W X
Y Z
------------
You have a keyboard layout as shown above in the XY plane, where each English uppercase letter is located at some coordinate, for example, 
the letter A is located at coordinate (0,0), the letter B is located at coordinate (0,1), the letter P is located at coordinate (2,3)
and the letter Z is located at coordinate (4,1).

Given the string word, return the minimum total distance to type such string using only two fingers. 
The distance between coordinates (x1,y1) and (x2,y2) is |x1 - x2| + |y1 - y2|. 

Note that the initial positions of your two fingers are considered free so don't count towards your total distance,
also your two fingers do not have to start at the first letter or the first two letters.

Example 1:

Input: word = "CAKE"
Output: 3
Explanation: 
Using two fingers, one optimal way to type "CAKE" is: 
Finger 1 on letter 'C' -> cost = 0 
Finger 1 on letter 'A' -> cost = Distance from letter 'C' to letter 'A' = 2 
Finger 2 on letter 'K' -> cost = 0 
Finger 2 on letter 'E' -> cost = Distance from letter 'K' to letter 'E' = 1 
Total distance = 3

My solution uses Dynamic Programming (DP) and recursion,
and beats 100% other solutions in memory.

Author Lucas Paz.
*/

internal struct Coordinates
{
    public int x;
    public int y;
    
    public Coordinates(int x, int y) => (this.x, this.y) = (x, y);
}

public class Solution 
{
    private string word;
    
    private static readonly IDictionary<char, Coordinates> lettersAndTheirCoordinates = new Dictionary<char, Coordinates>(26)
    {
        {'A', new Coordinates(0, 0)}, {'N', new Coordinates(2, 1)},
        {'B', new Coordinates(0, 1)}, {'O', new Coordinates(2, 2)},
        {'C', new Coordinates(0, 2)}, {'P', new Coordinates(2, 3)},
        {'D', new Coordinates(0, 3)}, {'Q', new Coordinates(2, 4)},
        {'E', new Coordinates(0, 4)}, {'R', new Coordinates(2, 5)},
        {'F', new Coordinates(0, 5)}, {'S', new Coordinates(3, 0)},
        {'G', new Coordinates(1, 0)}, {'T', new Coordinates(3, 1)},
        {'H', new Coordinates(1, 1)}, {'U', new Coordinates(3, 2)},
        {'I', new Coordinates(1, 2)}, {'V', new Coordinates(3, 3)},
        {'J', new Coordinates(1, 3)}, {'W', new Coordinates(3, 4)},
        {'K', new Coordinates(1, 4)}, {'X', new Coordinates(3, 5)},
        {'L', new Coordinates(1, 5)}, {'Y', new Coordinates(4, 0)},
        {'M', new Coordinates(2, 0)}, {'Z', new Coordinates(4, 1)}
    };
    
    //Dynamic Programming (DP) table
    //First dimension contains indices of first finger,
    //Second dimension contains indices of second finger:
    private int[ , ] minDistancesWhenFingersStartAtStringIndices;
    
    public int MinimumDistance(string word) 
    {
        int minDistance = 0;
        
        if (word == null || word.Length <= 2) 
            return 0;

        this.word = word;
        
        //Dynamic Programming (DP) table:
        this.minDistancesWhenFingersStartAtStringIndices = new int[word.Length, word.Length + 1];
        
        //Initialize DP table with -1:
        for (int i = 0; i < word.Length; i++)
            for (int j = 0; j <= word.Length; j++)
                minDistancesWhenFingersStartAtStringIndices[i, j] = -1;
        
        //Start recursion with DP. 
        //Arguments are the indices of the two fingers in the word, not in the table of letters.
        //First finger starts active, second finger starts not initialized.
        minDistance = MinDistanceFromIndices(0, -1);
        
        return minDistance;
    }
    
    private int MinDistanceFromIndices(int firstFingerIndexInWord, int secondFingerIndexInWord)
    {
        int minDistance = 0;
        
        if (firstFingerIndexInWord == word.Length - 1 || secondFingerIndexInWord == word.Length - 1)
            return 0;
        
        int secondIndex = (secondFingerIndexInWord == -1) ? word.Length : secondFingerIndexInWord;
        
        if (minDistancesWhenFingersStartAtStringIndices[firstFingerIndexInWord, secondIndex] != -1)
            return minDistancesWhenFingersStartAtStringIndices[firstFingerIndexInWord, secondIndex];
        
        int currentIndex = Math.Max(firstFingerIndexInWord, secondFingerIndexInWord);
        
        int costOfMovingFirstFinger = CalculateDistanceBeetweenLetters(firstFingerIndexInWord, currentIndex + 1)
            + MinDistanceFromIndices(currentIndex + 1, secondFingerIndexInWord);
        
        int costOfMovingSecondFinger = CalculateDistanceBeetweenLetters(secondFingerIndexInWord, currentIndex + 1)
            + MinDistanceFromIndices(firstFingerIndexInWord, currentIndex + 1);
        
        minDistance = Math.Min(costOfMovingFirstFinger, costOfMovingSecondFinger);
        
        minDistancesWhenFingersStartAtStringIndices[firstFingerIndexInWord, secondIndex]
            = minDistance;
        
        return minDistance;
    }
    
    private int CalculateDistanceBeetweenLetters(int sourceIndexInWord, int destinationIndexInWord)
    {
        int distance = 0;
        
        if (sourceIndexInWord < 0 || destinationIndexInWord < 0 || sourceIndexInWord == destinationIndexInWord) 
            return 0;
        
        char firstLetter = word[sourceIndexInWord],
             secondLetter = word[destinationIndexInWord];
        
        if (firstLetter == secondLetter) 
            return 0;
        
        Coordinates firstCoordinates = lettersAndTheirCoordinates[firstLetter],
                    secondCoordinates = lettersAndTheirCoordinates[secondLetter];
        
        int firstX = firstCoordinates.x, firstY = firstCoordinates.y,
            secondX = secondCoordinates.x, secondY = secondCoordinates.y;
        
        distance = Math.Abs(firstX - secondX) + Math.Abs(firstY - secondY);
        
        return distance;
    }
}
