namespace MatchBetting.NifsModels
{
    public class PlayerModel
    {
        public class BirthPlace
        {
            public string name { get; set; }
            public Country country { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
        }

        public class Championship
        {
            public string name { get; set; }
            public string fullName { get; set; }
            public int yearStart { get; set; }
            public int yearEnd { get; set; }
            public Tournament tournament { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
            public object externalIds { get; set; }
            public int sportId { get; set; }
        }

        public class Country
        {
            public string name { get; set; }
            public string shortName { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
            public object externalIds { get; set; }
        }

        public class Country2
        {
            public string name { get; set; }
            public string shortName { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
        }

        public class Data
        {
            public bool ratings { get; set; }
            public bool assists { get; set; }
            public bool attendances { get; set; }
            public bool corners { get; set; }
            public bool goalscorers { get; set; }
            public bool halfTimeScore { get; set; }
            public bool minutesPlayed { get; set; }
            public bool penalties { get; set; }
            public bool redCards { get; set; }
            public bool referees { get; set; }
            public bool shots { get; set; }
            public bool yellowCards { get; set; }
            public bool indirectAssists { get; set; }
            public bool transfers { get; set; }
            public bool robot { get; set; }
            public bool headCoaches { get; set; }
        }

        public class ExternalIds
        {
            public List<int> tv2 { get; set; }
            public List<int> enetpulse { get; set; }
            public PersonInTeam personInTeam { get; set; }
            public List<int> fiks { get; set; }
        }

        public class Kit
        {
            public List<string> colorCodes { get; set; }
            public int kitTypeId { get; set; }
            public string numberColor { get; set; }
            public int patternId { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
            public int sportId { get; set; }
        }

        public class Logo
        {
            public string url { get; set; }
            public int imageTypeId { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
        }

        public class Name
        {
            public string name { get; set; }
            public string use { get; set; }
            public int useId { get; set; }
            public object dateStart { get; set; }
            public object dateEnd { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
        }

        public class PersonInTeam
        {
            public List<int> enetpulse { get; set; }
        }

        public class Position
        {
            public string position { get; set; }
            public object x { get; set; }
            public int y { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
        }

        public class ProfilePicture
        {
            public string url { get; set; }
            public int imageTypeId { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
        }

        public class ProfilePicture2
        {
            public string url { get; set; }
            public int imageTypeId { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
        }

        public class Root
        {
            public string name { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public object nickName { get; set; }
            public List<Name> names { get; set; }
            public Position position { get; set; }
            public string birthDate { get; set; }
            public Country country { get; set; }
            public Country2 country2 { get; set; }
            public string gender { get; set; }
            public int height { get; set; }
            public bool isAReferee { get; set; }
            public object comment { get; set; }
            public BirthPlace birthPlace { get; set; }
            public List<Team> teams { get; set; }
            public object transfers { get; set; }
            public List<StageStatistic> stageStatistics { get; set; }
            public ProfilePicture profilePicture { get; set; }
            public List<ProfilePicture> profilePictures { get; set; }
            public string shirtName { get; set; }
            public Kit kit { get; set; }
            public int age { get; set; }
            public bool hiddenPersonInfo { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
            public ExternalIds externalIds { get; set; }
            public int sportId { get; set; }
        }

        public class Stage
        {
            public string name { get; set; }
            public string fullName { get; set; }
            public string groupName { get; set; }
            public int yearStart { get; set; }
            public int yearEnd { get; set; }
            public DateTime dateStart { get; set; }
            public DateTime dateEnd { get; set; }
            public Tournament tournament { get; set; }
            public int stageTypeId { get; set; }
            public int visibilityId { get; set; }
            public List<Championship> championships { get; set; }
            public Data data { get; set; }
            public string comment { get; set; }
            public int? numberOfRounds { get; set; }
            public int numberOfMeetingsBetweenTeams { get; set; }
            public object maxRoundNumber { get; set; }
            public bool active { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
            public object externalIds { get; set; }
            public int sportId { get; set; }
            public List<Name> names { get; set; }
        }

        public class StageStatistic
        {
            public Stage stage { get; set; }
            public Team team { get; set; }
            public int matches { get; set; }
            public int matchesStarting { get; set; }
            public int matchesStartingOnBench { get; set; }
            public int matchesComingOnFromBench { get; set; }
            public int matchesSubstituted { get; set; }
            public int minutes { get; set; }
            public int goals { get; set; }
            public int assists { get; set; }
            public int indirectAssists { get; set; }
            public int ownGoals { get; set; }
            public int yellowCards { get; set; }
            public int redCards { get; set; }
            public int matchesWon { get; set; }
            public int matchesDrawn { get; set; }
            public int matchesLost { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int? id { get; set; }
        }

        public class Team
        {
            public string name { get; set; }
            public string fromDate { get; set; }
            public object toDate { get; set; }
            public bool active { get; set; }
            public bool isInFantasySquad { get; set; }
            public int employmentId { get; set; }
            public object contractTypeId { get; set; }
            public int shirtNumber { get; set; }
            public Position position { get; set; }
            public bool clubTeam { get; set; }
            public Logo logo { get; set; }
            public object country { get; set; }
            public object activeTeam { get; set; }
            public object clubTeams { get; set; }
            public string primaryColor { get; set; }
            public string secondaryColor { get; set; }
            public string tertiaryColor { get; set; }
            public string textColor { get; set; }
            public object profilePictures { get; set; }
            public object hiddenPersonInfo { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
            public ExternalIds externalIds { get; set; }
            public int sportId { get; set; }
        }

        public class Team2
        {
            public string name { get; set; }
            public Country country { get; set; }
            public string homePage { get; set; }
            public string address { get; set; }
            public bool clubTeam { get; set; }
            public string gender { get; set; }
            public int? attendanceRecord { get; set; }
            public string dateFounded { get; set; }
            public object comment { get; set; }
            public object city { get; set; }
            public bool placeHolder { get; set; }
            public Logo logo { get; set; }
            public object teamPhoto { get; set; }
            public object stadiums { get; set; }
            public List<Name> names { get; set; }
            public object kits { get; set; }
            public object honours { get; set; }
            public object players { get; set; }
            public object staff { get; set; }
            public object teamStreak { get; set; }
            public bool isYouthTeam { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
            public object externalIds { get; set; }
            public int sportId { get; set; }
        }

        public class Tournament
        {
            public string name { get; set; }
            public string gender { get; set; }
            public int level { get; set; }
            public Country country { get; set; }
            public bool neutralVenues { get; set; }
            public int tournamentTypeId { get; set; }
            public int tournamentCategoryId { get; set; }
            public int visibilityId { get; set; }
            public string norwegianDescription { get; set; }
            public object names { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public int id { get; set; }
            public object externalIds { get; set; }
            public int sportId { get; set; }
        }


    }
}
