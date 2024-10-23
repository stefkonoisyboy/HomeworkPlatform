namespace Infrastructure
{
    public static class Constants
    {
        public const string ADMINISTRATOR_ROLE = "Admin";

        public const string TEACHER_ROLE = "Teacher";

        public const string STUDENT_ROLE = "Student";

        public const string CLASS_NOT_FOUND = "Class not found";

        public const string USER_NOT_FOUND = "User not found";

        public const string HOMEWORK_NOT_FOUND = "Homework not found";

        public const string HOMEWORK_SUBMISSION_NOT_FOUND = "Homework Submission not found";

        public const string GRADE_NOT_FOUND = "Grade not found";

        public const string CURRENT_USER_NOT_MATCHING = "You cannot fetch other user's info";

        public const string GRADE_ALREADY_CREATED = "Grade already exists for this homework submission";

        public const string CANNOT_GRADE_HOMEWORK_SUBMISSION_WITHOUT_POINTS = "You cannot grade a homework submission for a homework that has no points assigned to it";

        public const string GRADE_POINTS_EXCEEDED = "You cannot grade a homework submission with more points that the corresponding homework is assigned with";

        public const string POINTS_SHOULD_BE_NUMERIC = "Points should be numeric";

        public const string STUDENT_ALREADY_ASSIGNED = "This student has been already assigned for this class";

        public const string TEACHER_ALREADY_ASSIGNED = "This teacher has been already assigned for this class";

        public const string NON_EXISTENT_STUDENT_IN_CLASS = "This student does not exist in this class";

        public const string NON_EXISTENT_TEACHER_IN_CLASS = "This teacher does not exist in this class";

        public const string CANNOT_REMOVE_CREATOR_TEACHER = "You are not allowed to remove the creator of the class";
    }
}
