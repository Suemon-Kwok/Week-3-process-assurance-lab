namespace ENSE707_AppointmentBooking
{
    public class Patient
    {
        // Read-only after construction, same reasoning as Doctor.Id -
        // a patient's system ID shouldn't be reassignable once created.
        public string Id { get; }

        // The patient's official/legal name (e.g. as it appears on
        // insurance or medical records). Kept separate from PreferredName
        // below so the system never loses the legal identity even if
        // a friendlier name is also stored.
        public string LegalName { get; }

        // Optional - a name the patient wants to be addressed by, which
        // may differ from their legal name (nickname, cultural naming
        // convention, gender-affirming name, etc.). This directly
        // supports the "cultural quality" risk noted in the original
        // Patient class, which assumed one rigid naming format.
        public string PreferredName { get; }

        // A computed property (no backing field) - it doesn't store
        // anything itself, it derives its value from LegalName and
        // PreferredName every time it's read. This means DisplayName
        // can never get out of sync with the two names it's based on.
        public string DisplayName
        {
            get
            {
                // If no preferred name was given, fall back to the legal
                // name so the system always has SOMETHING to display -
                // reception staff are never shown a blank name.
                if (string.IsNullOrWhiteSpace(PreferredName))
                    return LegalName;

                // Otherwise, prefer the name the patient actually wants
                // to be called - this is what makes booking messages
                // feel personal and respectful rather than clinical.
                return PreferredName;
            }
        }

        // preferredName defaults to "" so callers who don't have a
        // preferred name on file can still create a valid Patient
        // without being forced to pass a third argument every time.
        public Patient(string id, string legalName, string preferredName = "")
        {
            // Same defensive pattern as Doctor - reject blank/whitespace
            // IDs immediately so an "empty" patient can never exist.
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Patient ID is required.");

            // A patient must have SOME legal name on record - this is
            // treated as mandatory, unlike PreferredName which is optional.
            if (string.IsNullOrWhiteSpace(legalName))
                throw new ArgumentException("Legal name is required.");

            // No validation needed here - an empty/missing preferredName
            // is a perfectly valid state (DisplayName just falls back
            // to LegalName in that case, handled above).
            Id = id;
            LegalName = legalName;
            PreferredName = preferredName;
        }
    }
}