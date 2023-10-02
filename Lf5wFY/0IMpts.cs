using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Sample user behavior data (user_id, item_id, interaction_score)
        var userInteractions = new List<UserInteraction>
        {
            new UserInteraction { UserId = 1, ItemId = 101, InteractionScore = 5 },
            new UserInteraction { UserId = 1, ItemId = 102, InteractionScore = 4 },
            new UserInteraction { UserId = 2, ItemId = 101, InteractionScore = 5 },
            new UserInteraction { UserId = 2, ItemId = 103, InteractionScore = 3 },
            new UserInteraction { UserId = 3, ItemId = 102, InteractionScore = 4 }
        };

        // Calculate user profiles based on their interactions
        var userProfiles = CalculateUserProfiles(userInteractions);

        // Recommend items for a user
        int userIdToRecommend = 1;
        var recommendedItems = RecommendItems(userIdToRecommend, userInteractions, userProfiles, topN: 5);

        Console.WriteLine($"Recommended items for user {userIdToRecommend}: {string.Join(", ", recommendedItems)}");
    }

    static Dictionary<int, double> CalculateUserProfiles(List<UserInteraction> interactions)
    {
        var userProfiles = new Dictionary<int, double>();
        var userInteractionsCount = new Dictionary<int, int>();

        foreach (var interaction in interactions)
        {
            if (!userProfiles.ContainsKey(interaction.UserId))
            {
                userProfiles[interaction.UserId] = 0;
                userInteractionsCount[interaction.UserId] = 0;
            }

            userProfiles[interaction.UserId] += interaction.InteractionScore;
            userInteractionsCount[interaction.UserId]++;
        }

        foreach (var userId in userProfiles.Keys.ToList())
        {
            userProfiles[userId] /= userInteractionsCount[userId];
        }

        return userProfiles;
    }

    static List<int> RecommendItems(int userId, List<UserInteraction> interactions, Dictionary<int, double> userProfiles, int topN)
    {
        var userInteractions = interactions.Where(i => i.UserId == userId).Select(i => i.ItemId).ToList();
        var recommendedItems = new List<int>();

        foreach (var interaction in interactions)
        {
            if (!userInteractions.Contains(interaction.ItemId))
            {
                recommendedItems.Add(interaction.ItemId);
            }
        }

        recommendedItems = recommendedItems.OrderByDescending(itemId => userProfiles[userId]).Take(topN).ToList();
        return recommendedItems;
    }
}

class UserInteraction
{
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public double InteractionScore { get; set; }
}
