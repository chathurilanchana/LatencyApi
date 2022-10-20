using FluentValidation;
using LatencyService.Api.Modals;
using System.Globalization;

namespace LatencyService.Api.Validators
{
    public class LatencyRequestModalValidator : AbstractValidator<LatencyRequestModal>
    {
       public LatencyRequestModalValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.StartDate).Must(IsValidDateTime).WithMessage("Start date should be in YYYY-MM-dd format and year should be 2021.");
            RuleFor(x => x.EndDate).Must(IsValidDateTime).WithMessage("End date should be in YYYY-MM-dd format and year should be 2021.");
            RuleFor(x => new { x.StartDate, x.EndDate }).Must(model => IsLessThanEndDate(model.StartDate, model.EndDate)).WithMessage("Start date must be less than end date.");
            RuleFor(x => new { x.StartDate, x.EndDate }).Must(model => IsDifferenceLessThan30Days(model.StartDate, model.EndDate)).WithMessage("Upto 30 days data fetching is allowed.");

        }

        private bool IsDifferenceLessThan30Days(string startDate, string endDate)
        {
            var startDateInDateFormat = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endDateInDateFormat = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return (endDateInDateFormat - startDateInDateFormat).TotalDays <= 30;
        }

        private bool IsLessThanEndDate(string startDate, string endDate)
        {
            var startDateInDateFormat = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endDateInDateFormat = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return startDateInDateFormat <= endDateInDateFormat;
        }

        private bool IsValidDateTime(string date)
        {
            var isValidDate = DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var startDateInDateFormat);

            return isValidDate && startDateInDateFormat.Year == 2021;
        }
    }
}
