using System;
using System.Collections.Generic;
using System.Linq;

public class Judge : IJudge
{
    private HashSet<int> users;
    private HashSet<int> contests;
    private HashSet<Submission> submissions;
    private Dictionary<int, Submission> bySubmissionId;
    private Dictionary<SubmissionType, HashSet<Submission>> bySubmissionType;
    private Dictionary<int, Submission> byUserId;

    public Judge()
    {
        this.users = new HashSet<int>();
        this.contests = new HashSet<int>();
        this.submissions = new HashSet<Submission>();
        this.bySubmissionId = new Dictionary<int, Submission>();
        this.bySubmissionType = new Dictionary<SubmissionType, HashSet<Submission>>();
        this.byUserId = new Dictionary<int, Submission>();
    }
    public void AddContest(int contestId)
    {
        this.contests.Add(contestId);
    }

    public void AddSubmission(Submission submission)
    {
        if (!this.users.Contains(submission.UserId) || !this.contests.Contains(submission.ContestId))
        {
            throw new InvalidOperationException();
        }
        this.submissions.Add(submission);
        if (!this.bySubmissionType.ContainsKey(submission.Type))
        {
            this.bySubmissionType.Add(submission.Type, new HashSet<Submission>());
        }
        this.bySubmissionType[submission.Type].Add(submission);
    }

    public void AddUser(int userId)
    {
        this.users.Add(userId);
    }

    public void DeleteSubmission(int submissionId)
    {
        this.submissions.Remove(this.bySubmissionId[submissionId]);
    }

    public IEnumerable<Submission> GetSubmissions()
    {
        return this.submissions.OrderBy(x => x);
    }

    public IEnumerable<int> GetUsers()
    {
        return this.users.OrderBy(x => x);
    }

    public IEnumerable<int> GetContests()
    {
        return this.contests.OrderBy(x => x);
    }

    public IEnumerable<Submission> SubmissionsWithPointsInRangeBySubmissionType(int minPoints, int maxPoints, SubmissionType submissionType)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<int> ContestsByUserIdOrderedByPointsDescThenBySubmissionId(int userId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Submission> SubmissionsInContestIdByUserIdWithPoints(int points, int contestId, int userId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<int> ContestsBySubmissionType(SubmissionType submissionType)
    {
        return this.bySubmissionType[submissionType].Select(x=>x.ContestId);
    }
}
