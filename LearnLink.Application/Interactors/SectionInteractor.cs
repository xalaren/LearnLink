using LearnLink.Application.Mappers;
using LearnLink.Core.Exceptions;
using LearnLink.Shared.DataTransferObjects;
using LearnLink.Shared.Responses;

namespace LearnLink.Application.Interactors
{
    public class SectionInteractor
    {

        public async Task<Response> CreateSectionAsync(SectionDto sectionDto)
        {
            try
            {
                var section = sectionDto.ToEntity();

                return new Response()
                {
                    Success = true,
                    StatusCode = 200,
                };
            }
            catch (CustomException exception)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = exception.StatusCode,
                    Message = exception.Message,
                };
            }
            catch (Exception exception)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Не удалось создать раздел",
                    InnerErrorMessages = new string[] { exception.Message },
                };
            }
        }
    }
}
