using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class IntegrationData
    {
    }
    public class Assignment
    {
        public int id { get; set; }
        public string description { get; set; }
        public object due_at { get; set; }
        public object unlock_at { get; set; }
        public object lock_at { get; set; }
        public string points_possible { get; set; }
        public string grading_type { get; set; }
        public int assignment_group_id { get; set; }
        public object grading_standard_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public bool peer_reviews { get; set; }
        public bool automatic_peer_reviews { get; set; }
        public int position { get; set; }
        public bool grade_group_students_individually { get; set; }
        public bool anonymous_peer_reviews { get; set; }
        public object group_category_id { get; set; }
        public bool post_to_sis { get; set; }
        public bool moderated_grading { get; set; }
        public bool omit_from_final_grade { get; set; }
        public bool intra_group_peer_reviews { get; set; }
        public bool anonymous_instructor_annotations { get; set; }
        public bool anonymous_grading { get; set; }
        public bool graders_anonymous_to_graders { get; set; }
        public int grader_count { get; set; }
        public bool grader_comments_visible_to_graders { get; set; }
        public object final_grader_id { get; set; }
        public bool grader_names_visible_to_final_grader { get; set; }
        public int allowed_attempts { get; set; }
        public string secure_params { get; set; }
        public int course_id { get; set; }
        public string name { get; set; }
        public IList<string> submission_types { get; set; }
        public bool has_submitted_submissions { get; set; }
        public bool due_date_required { get; set; }
        public int max_name_length { get; set; }
        public bool in_closed_grading_period { get; set; }
        public bool is_quiz_assignment { get; set; }
        public bool can_duplicate { get; set; }
        public object original_course_id { get; set; }
        public object original_assignment_id { get; set; }
        public object original_assignment_name { get; set; }
        public string workflow_state { get; set; }
        public bool muted { get; set; }
        public string html_url { get; set; }
        public bool has_overrides { get; set; }
        public int needs_grading_count { get; set; }
        public object sis_assignment_id { get; set; }
        public object integration_id { get; set; }
        public IntegrationData integration_data { get; set; }
        public int quiz_id { get; set; }
        public bool anonymous_submissions { get; set; }
        public bool published { get; set; }
        public bool unpublishable { get; set; }
        public bool only_visible_to_overrides { get; set; }
        public bool locked_for_user { get; set; }
        public string submissions_download_url { get; set; }
        public bool anonymize_students { get; set; }
    }
}