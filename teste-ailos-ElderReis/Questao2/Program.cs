using Newtonsoft.Json;
using Questao2.Model;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoalsAsync(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoalsAsync(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoalsAsync(string team, int year)
    {
        var totalGoalsWhentTeam1 = await getTotalScoredGoalsPerTeamAsync(team, year, 1);
        var totalGoalsWhentTeam2 = await getTotalScoredGoalsPerTeamAsync(team, year, 2);
        return totalGoalsWhentTeam1 + totalGoalsWhentTeam2;
    }

    public static async Task<int> getTotalScoredGoalsPerTeamAsync(string team, int year, int teamNumber)
    {

        var totalGoals = 0;
        using (var httpClient = new HttpClient())
        {
            var page = 1;
            var totalPages = 1;
            do
            {
                var url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team{teamNumber}={team}&page={page}";
                var response = await httpClient.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();
                var matches = JsonConvert.DeserializeObject<MatchResult>(responseBody);
                int goals = 0;
                if (teamNumber == 1)
                    goals = matches!.Matches!.Sum(s => s.Team1Score);
                if (teamNumber == 2)
                    goals = matches!.Matches!.Sum(s => s.Team2Score);
                totalGoals += goals;
                totalPages = matches!.TotalPages;
                page++;
            } while (page <= totalPages);
        }
        return totalGoals;
    }

}