
public class DifficultyHandler : BaseBehaviour {

	private int difficultyLevel;

	void OnSetDifficulty(int incoming)
	{
		difficultyLevel = incoming;

		switch (difficultyLevel)
		{
			case NORMAL:
				EventKit.Broadcast("set difficulty damage modifier", 1);
				break;
			case HARD:
				EventKit.Broadcast("set difficulty damage modifier", 2);
				break;
			case KILL_ME_NOW:
				EventKit.Broadcast("set difficulty damage modifier", 3);
				break;
		}
	}

	void OnEnable()
	{
		EventKit.Subscribe<int>("set difficulty", OnSetDifficulty);
	}

	void OnDestroy()
	{
		EventKit.Unsubscribe<int>("set difficulty", OnSetDifficulty);
	}
}
