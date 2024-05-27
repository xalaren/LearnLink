using LearnLink.Application.Interactors;
using LearnLink.Shared.Responses;
using LearnLink.WebApi.Pages.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearnLink.WebApi.Pages.Completions;

public class CompleteLesson(CompletionInteractor completionInteractor) : CompletionsBasePageModel(completionInteractor)
{
    public Response? QueryResult { get; set; }
    
    public IActionResult OnGet()
    {
        return AuthRequired();
    }

    public async Task OnPost(int userId, int courseId, int moduleId, int lessonId, string complete)
    {
        var completeValue = !string.IsNullOrWhiteSpace(complete);
        QueryResult = await CompletionInteractor.ChangeLessonCompleted(userId, courseId, moduleId, lessonId, completeValue);
    }
    
}