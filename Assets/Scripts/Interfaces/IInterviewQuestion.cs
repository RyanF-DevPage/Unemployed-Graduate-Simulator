public interface IInterviewQuestion
{
    string QuestionText { get; }
    string[] Choices { get; }
    int CorrectChoiceIndex { get; }     // Weighted by Career Skills investment
    float SuccessWeight { get; }        // Contribution to overall pass/fail score
}
