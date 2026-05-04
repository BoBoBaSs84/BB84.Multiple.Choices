using BB84.Multiple.Choices.Core.Settings;

namespace BB84.Multiple.Choices.Core.Tests.Settings;

[TestClass]
public sealed class QuizSettingsTests
{
	[TestMethod]
	public void QuizSettingsShouldHaveDefaultValues()
	{
		QuizSettings? settings;

		settings = new QuizSettings();

		Assert.IsNotNull(settings);
		Assert.AreEqual("sampleQuestions.json", settings.QuestionsFilePath);
		Assert.AreEqual(50, settings.QuestionsPerQuiz);
		Assert.AreEqual(5, settings.QuestionsPerRound);
		Assert.IsFalse(settings.RandomizeQuestions);
		Assert.AreEqual(0.5f, settings.ThresholdScorePerRound);
	}
}
