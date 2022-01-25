using System.Collections.Generic;
using System.Linq;

namespace Filmster.Helpers
{
    public static class VoteHelper
    {
        public static (double VoteAverage, int VoteCount) GetVoteAverageVoteCount(IEnumerable<(double VoteAverage, int VoteCount)> votes)
        {
            var voteAverage = votes.Where(vote => vote.VoteAverage > 0).DefaultIfEmpty().Average(vote => vote.VoteAverage);
            var voteCount = votes.Sum(vote => vote.VoteCount);
            return (voteAverage, voteCount);
        }
    }
}
