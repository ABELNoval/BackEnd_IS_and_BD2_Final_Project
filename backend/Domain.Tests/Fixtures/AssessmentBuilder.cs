using System;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests.Fixtures
{
	public class AssessmentBuilder
	{
		private Guid _technicalId = Guid.NewGuid();
		private Guid _directorId = Guid.NewGuid();
		private PerformanceScore _score = PerformanceScore.Create(4.5m);
		private string _comment = "Buen desempeño";
		private DateTime _assessmentDate = DateTime.UtcNow.AddDays(-2);

		public AssessmentBuilder WithTechnicalId(Guid id)
		{
			_technicalId = id;
			return this;
		}
		public AssessmentBuilder WithDirectorId(Guid id)
		{
			_directorId = id;
			return this;
		}
		public AssessmentBuilder WithScore(PerformanceScore score)
		{
			_score = score;
			return this;
		}
		public AssessmentBuilder WithComment(string comment)
		{
			_comment = comment;
			return this;
		}
		public AssessmentBuilder WithAssessmentDate(DateTime date)
		{
			_assessmentDate = date;
			return this;
		}
		public Assessment Build()
		{
			return Assessment.Create(_technicalId, _directorId, _score, _comment);
		}
	}
}
