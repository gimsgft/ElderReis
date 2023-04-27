using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao2.Model
{
    public class MatchResult
    {
        [JsonProperty("page")]
        public int PageNumber { get; set; }

        [JsonProperty("per_page")]
        public int ResultsPerPage { get; set; }

        [JsonProperty("total")]
        public int TotalResults { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("data")]
        public List<Match>? Matches { get; set; }
    }

    public class Match
    {
        [JsonProperty("competition")]
        public string? CompetitionName { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("round")]
        public string? RoundName { get; set; }

        [JsonProperty("team1")]
        public string? Team1Name { get; set; }

        [JsonProperty("team2")]
        public string? Team2Name { get; set; }

        [JsonProperty("team1goals")]
        public int Team1Score { get; set; }

        [JsonProperty("team2goals")]
        public int Team2Score { get; set; }
    }
}
