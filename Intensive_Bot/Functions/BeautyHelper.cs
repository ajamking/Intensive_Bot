using Intensive_Bot.EntitiesAndModels;
using System.Text;

namespace Intensive_Bot.BLFunctions;

public static class BeautyHelper
{
    private static string _beautydivider = "• • • • • • • • • • •";

    public static string DividerToken = "DividerToken";

    public static string MakeMrResponseBeautier(this List<MergeRequestInfoUI> mrList)
    {
        var answer = new StringBuilder();

        var count = 0;

        foreach (var mr in mrList)
        {
            answer.AppendLine(MakeItStyled($"ID проекта: {mr.ProjectId}", UiTextStyle.Default));
            answer.AppendLine(MakeItStyled($"Описание: {mr.Description}", UiTextStyle.Subtitle));
            answer.AppendLine(_beautydivider);
            answer.AppendLine(MakeItStyled($"Cоздан: {mr.CreatedAt.ChangeToUiDateTime()}", UiTextStyle.Default));
            answer.AppendLine(MakeItStyled($"Обновлен: {mr.UpdatedAt.ChangeToUiDateTime()}", UiTextStyle.Default));
            answer.AppendLine(MakeItStyled($"Статус: {mr.State}", UiTextStyle.Default));
            answer.AppendLine(MakeItStyled($"Статус cлияния: {mr.MergeStatus}", UiTextStyle.Name));
            answer.AppendLine(_beautydivider);
            answer.AppendLine(MakeItStyled($"Целевая ветка: {mr.TargetBranch}", UiTextStyle.Subtitle));
            answer.AppendLine(MakeItStyled($"Исходная ветка: {mr.SourceBranch}", UiTextStyle.Subtitle));
            answer.AppendLine(_beautydivider);
            answer.AppendLine(MakeItStyled($"Автор: {mr.Author.Name}-{mr.Author.Username}", UiTextStyle.Name));
            answer.AppendLine(MakeItStyled($"Закреплены: {string.Join("; ", mr.Assignees.Select(a => $"{a.Name}-{a.Username}"))}", UiTextStyle.Name));
            answer.AppendLine(MakeItStyled($"Назначен: {mr.Assignee}", UiTextStyle.Name));
            answer.AppendLine(MakeItStyled($"Рецензенты: {string.Join("; ", mr.Reviewers.Select(r => $"{r.Name}-{r.Username}"))}", UiTextStyle.Name));
            answer.AppendLine(_beautydivider);
            answer.AppendLine(MakeItStyled($"Ссылка на MR: {mr.WebUrl}", UiTextStyle.Default));

            count++;

            if (count < mrList.Count)
            {
                answer.AppendLine($"\n{DividerToken}");
            }
        }

        return answer.ToString();
    }

    public static string ChangeToUiDateTime(this string apiDateTime)
    {
        var isCorrectString = DateTime.TryParse(apiDateTime, out DateTime dt);

        if (isCorrectString)
        {
            return $"{dt:dd\\ MMM} в {dt:HH:mm}";
        }

        return $"Unknown DateTime format";
    }

    public static string MakeItStyled(string str, UiTextStyle textStyle)
    {
        return textStyle switch
        {
            UiTextStyle.Header => $@"_*{Ecranize(str)}*_".ToUpper(),
            UiTextStyle.Subtitle => $@"_*{Ecranize(str)}*_",
            UiTextStyle.TableAnnotation => $@"__*{Ecranize(str)}*__",
            UiTextStyle.Name => $@"*{Ecranize(str)}*",
            UiTextStyle.Default => Ecranize(str),
            _ => Ecranize($@"Text Style Error"),
        };
    }

    public static string Ecranize(string str)
    {
        var reservedSymbols = @"_*,`.[]()~'><#+-/=|{}!""№;%:?*\";

        if (!str.Any(x => reservedSymbols.Contains(x)))
        {
            return str;
        }

        StringBuilder newStr = new(str.Length);

        foreach (var c in str)
        {
            if (reservedSymbols.Contains(c))
            {
                newStr.Append('\\');
                newStr.Append(c);
            }
            else
            {
                newStr.Append(c);
            }
        }

        return newStr.ToString();
    }
}