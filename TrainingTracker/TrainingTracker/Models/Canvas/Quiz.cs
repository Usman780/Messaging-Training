using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainingTracker.Models
{
    public class CanvasQuiz
    {
        public int id { get; set; }
        public string title { get; set; }
        public string html_url { get; set; }
        public string mobile_url { get; set; }
        public string description { get; set; }
        public string quiz_type { get; set; }
        public object time_limit { get; set; }
        public bool shuffle_answers { get; set; }
        public bool show_correct_answers { get; set; }
        public string scoring_policy { get; set; }
        public int allowed_attempts { get; set; }
        public bool one_question_at_a_time { get; set; }
        public int question_count { get; set; }
        public double points_possible { get; set; }
        public bool cant_go_back { get; set; }
        public object access_code { get; set; }
        public object ip_filter { get; set; }
        public object due_at { get; set; }
        public object lock_at { get; set; }
        public object unlock_at { get; set; }
        public bool published { get; set; }
        public bool unpublishable { get; set; }
        public bool locked_for_user { get; set; }
        public LockInfo lock_info { get; set; }
        public string lock_explanation { get; set; }
        public object hide_results { get; set; }
        public object show_correct_answers_at { get; set; }
        public object hide_correct_answers_at { get; set; }
        public IList<AllDate> all_dates { get; set; }
        public bool can_unpublish { get; set; }
        public bool can_update { get; set; }
        public bool require_lockdown_browser { get; set; }
        public bool require_lockdown_browser_for_results { get; set; }
        public bool require_lockdown_browser_monitor { get; set; }
        public string lockdown_browser_monitor_data { get; set; }
        public string speed_grader_url { get; set; }
        public Permissions permissions { get; set; }
        public string quiz_reports_url { get; set; }
        public string quiz_statistics_url { get; set; }
        public string message_students_url { get; set; }
        public int section_count { get; set; }
        public string quiz_submission_versions_html_url { get; set; }
        public string assignment_id { get; set; }
        public bool one_time_results { get; set; }
        public bool only_visible_to_overrides { get; set; }
        public string assignment_group_id { get; set; }
        public bool show_correct_answers_last_attempt { get; set; }
        public int version_number { get; set; }
        public bool has_access_code { get; set; }
      //  public bool post_to_sis { get; set; }
        public string migration_id { get; set; }

    }
    public class LockInfo
    {
        public string missing_permission { get; set; }
        public string asset_string { get; set; }
    }

    public class AllDate
    {
        public object due_at { get; set; }
        public object unlock_at { get; set; }
        public object lock_at { get; set; }
}

    public class Permissions
    {
        public bool read_statistics { get; set; }
        public bool manage { get; set; }
        public bool read { get; set; }
        public bool update { get; set; }
        public bool create { get; set; }
        public bool submit { get; set; }
        public bool preview { get; set; }
        public bool delete { get; set; }
        public bool grade { get; set; }
        public bool review_grades { get; set; }
        public bool view_answer_audits { get; set; }

    }
}