using CC.Common.Errors;
using CC.TechSupportService.Domain.ValueObjects.Comment;
using CC.TechSupportService.Domain.ValueObjects.FileDetails;
using CC.TechSupportService.Domain.ValueObjects.Ticket;
using FluentResults;

namespace CC.TechSupportService.Domain.Enums;

public static class TechServiceErrors
{
    // TS-1xx - ошибки создания тикета
    public static Error NegativeTicketNumber
        => new Error("Номер заявки не может быть отрицательным")
            .WithErrorCode("TS-100");
    
    public static Error CsatCommentIsEmpty
        => new Error("Комментарий не может быть пустым")
            .WithErrorCode("TS-101");

    public static Error CsatCommentLengthExceeded
        => new Error($"Длина комментария не может превышать {CsatComment.MaxCommentLength} символов")
            .WithErrorCode("TS-102");

    public static Error CsatCommentLengthTooShort
        => new Error($"Длина комментария не может быть меньше {CsatComment.MinCommentLength} символов")
            .WithErrorCode("TS-103");
    
    public static Error CsatRatingExceedsMaxValue
        => new Error("Рейтинг не может быть выше 5")
            .WithErrorCode("TS-104");
    
    public static Error TicketDescriptionIsEmpty 
        => new Error("Описание тикета не может быть пустым")
            .WithErrorCode("TS-105");

    public static Error TicketDescriptionIsTooLong 
        => new Error($"Описание тикета не может превышать {TicketDescription.MaxDescriptionLenght} символов")
            .WithErrorCode("TS-106");
    
    public static Error TicketSubjectIsEmpty 
        => new Error("Тема тикета не может быть пустой")
            .WithErrorCode("TS-107");

    public static Error TicketSubjectIsTooLong 
        => new Error($"Тема тикета не может превышать {TicketSubject.MaxSubjectLenght} символов")
            .WithErrorCode("TS-108");
    
    // TS-2xx - ошибки изменения тикета
    public static Error ChangingAgentThatHasNotBeenAssigned
        => new Error("Невозможно изменить Агента когда он не был назначен")
            .WithErrorCode("TS-200");

    public static Error SettingTheSameAssignedTicketAgent
        => new Error("Новый Агент тот же что и старый")
            .WithErrorCode("TS-201");

    public static Error SettingFirstAgentRespondedTimeForTheSecondTime
        => new Error("Первое время ответа Агента уже было установлено")
            .WithErrorCode("TS-202");

    public static Error LastUserRespondedTimeIsBeforeCurrent
        => new Error("Новое время ответа пользователя не может быть раньше текущего")
            .WithErrorCode("TS-203");
    
    public static Error SettingTheSameTicketDescription
        => new Error("Новое описание тикета не может совпадать с текущим")
            .WithErrorCode("TS-204");

    public static Error TimeToEditTicketDescriptionHasPassed
        => new Error("Время для редактирования описания тикета истекло")
            .WithErrorCode("TS-205");
    
    public static Error ReclassifyingTicketWithUnassignedAgent
        => new Error("Невозможно изменить категорию заявки без назначенного Агента")
            .WithErrorCode("TS-206");
    
    public static Error ChangingRatingAndCommentAfterPosting
        => new Error("Невозможно изменить оценку и комментарий после публикации")
            .WithErrorCode("TS-207");
    
    public static Error OpeningFromStatus(string statusName)
        => new Error($"Невозможно открыть тикет из статуса {statusName}")
            .WithErrorCode("TS-208");
    
    public static Error AssigningInProgressFromStatus(string statusName)
        => new Error($"Невозможно перевести в работу из статуса {statusName}")
            .WithErrorCode("TS-209");
    
    public static Error ShiftingToWaitingForUserFromStatus(string statusName)
        => new Error($"Невозможно перевести в ожидание пользователя из статуса {statusName}")
            .WithErrorCode("TS-210");
    
    public static Error EscalatingFromStatus(string statusName)
        => new Error($"Невозможно эскалировать тикет из статуса {statusName}")
            .WithErrorCode("TS-211");
    
    public static Error ResolvingFromStatus(string statusName)
        => new Error($"Невозможно разрешить тикет из статуса {statusName}")
            .WithErrorCode("TS-212");
    
    public static Error ReopeningFromStatus(string statusName)
        => new Error($"Невозможно переоткрыть тикет из статуса {statusName}")
            .WithErrorCode("TS-213");
    
    public static Error ClosingFromStatus(string statusName)
        => new Error($"Невозможно закрыть тикет из статуса {statusName}")
            .WithErrorCode("TS-214");
    
    // TS-3xx - Ошибки Вложения тикета
    public static Error PassingTheSameAttachmentIdAsTicketId
        => new Error($"Айди вложения тикета не может совпадать с айди тикета")
            .WithErrorCode("TS-300");
    
    // TS-4xx - Ошибки деталей файла
    public static Error PassingNegativeContentSize
        => new Error($"Размер содержимого не может быть отрицательным")
            .WithErrorCode("TS-400");
    
    public static Error PassingTheSameFileDetailsIdAsAttachmentId
        => new Error($"Айди деталей файла не может совпадать с айди вложения тикета")
            .WithErrorCode("TS-401");
    
    public static Error FileNameCannotBeEmpty
        => new Error("Имя файла не может быть пустым")
            .WithErrorCode("TS-402");

    public static Error FileNameLengthExceeded
        => new Error($"Длина имени файла не может превышать {FileName.MaxFileNameLength} символов")
            .WithErrorCode("TS-403");
    
    public static Error ContentTypeIsEmpty 
        => new Error("Тип контента не может быть пустым")
            .WithErrorCode("TS-404");

    public static Error ContentTypeIsNotMimeType 
        => new Error("Переданная строка должна быть в формате MIME-типа")
            .WithErrorCode("TS-405");

    public static Error ContentTypeIsTooLong 
        => new Error($"Длина типа контента не может превышать {ContentType.MaxContentTypeLength} символов")
            .WithErrorCode("TS-406");
    
    // TS-5xx - Ошибки Комментария
    public static Error PassingTheSameTicketIdAsCommentId
        => new Error($"Айди комментария не может совпадать с айди тикета")
            .WithErrorCode("TS-500");
    
    public static Error EmptyCommentBody
        => new Error("Содержимое не может быть пустым")
            .WithErrorCode("TS-501");

    public static Error CommentBodyLengthExceeded
        => new Error($"Длина содержимого не может превышать {CommentBody.MaxBodyLenght} символов")
            .WithErrorCode("TS-502");

    public static Error BodyLengthTooShort
        => new Error($"Длина содержимого не может быть меньше минимального порога в {CommentBody.MinBodyLenght} символов")
            .WithErrorCode("TS-503");
    
    // TS-6xx - Ошибки Истории тикета
    public static Error PassingTheSameTicketIdAsActorId
        => new Error($"Айди тикета не может совпадать с айди редактора")
            .WithErrorCode("TS-600");
    
    //TS-7xx - Ошибки Запроса пользователя
    public static Error PassingLastRequestTimeFromFuture
        => new Error($"Время последнего запроса не может быть в будущем")
            .WithErrorCode("TS-700");
}