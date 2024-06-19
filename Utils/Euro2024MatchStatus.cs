namespace MatchBetting.Utils
{
    public class Euro2024MatchStatus
    {
        private static readonly Dictionary<int, string> matchStatusDictionary = new Dictionary<int, string>
        {
            { 1, "Spilt" },
            { 2, "Ikke startet (Også 'resultat ukjent')" },
            { 3, "Utsatt" },
            { 4, "Avbrutt" },
            { 5, "Vil ikke bli spilt" },
            { 6, "Dato ikke satt" },
            { 7, "Pågående" },
            { 8, "Første omgang" },
            { 9, "Pause" },
            { 10, "Andre omgang" },
            { 11, "Første ekstraomgang" },
            { 12, "Andre ekstraomgang" },
            { 13, "Straffesparkkonkurranse" },
            { 14, "Pause før ekstraomgang" },
            { 15, "Pause i ekstraomgang" },
            { 16, "Spilt, men kansellert" },
            { 17, "Spilt, men teller ikke i statistikken" },
            { 18, "Andre ekstraomgang" },
            { 19, "Tredje ekstraomgang" },
            { 20, "Pause i tredje ekstraomgang" },
            { 21, "Fjerde ekstraomgang" },
            { 22, "Tredje omgang" },
            { 23, "Pause i fjerde ekstraomgang" },
            { 24, "Femte ekstraomgang" },
            { 25, "Pause i femte ekstraomgang" },
            { 26, "Sjette ekstraomgang" },
            { 27, "Pause i sjette ekstraomgang" },
            { 28, "Syvende ekstraomgang" },
            { 29, "Pause i syvende ekstraomgang" },
            { 30, "Åttende ekstraomgang" },
            { 31, "Andre omgang" }
        };

        public static string GetMatchStatusText(int matchStatusId)
        {
            if (matchStatusDictionary.TryGetValue(matchStatusId, out string statusText))
            {
                return statusText;
            }
            else
            {
                return "Ukjent status";
            }
        }

        public static bool MatchIsActive(int matchStatusId)
        {
            return (matchStatusId >= 7 && matchStatusId <= 15) || matchStatusId >= 18;
        }
    }
}
