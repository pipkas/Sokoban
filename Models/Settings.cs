namespace Sokoban.Models;

public class Settings
{
    public int GameCellSize { get; set; }
    public Language Language { get; set; }
    public int WindowHeight { get; set; }
    public int WindowWidth { get; set; }
    public Dictionary<Language, Dictionary<string, string>> Translations { get; private set; }

    public Settings()
    {
        GameCellSize = 60;
        Language = Language.Russian;
        WindowHeight = 700;
        WindowWidth = 1000;
        Translations = InitTranslations();
    }

    private Dictionary<Language, Dictionary<string, string>> InitTranslations()
    {
        return new Dictionary<Language, Dictionary<string, string>>
        {
            [Language.Russian] = new Dictionary<string, string>
            {
                ["time"] = "Время",
                ["youAreInPlace"] = "Вы на {0} месте :)",
                ["back to levels"] = "К уровням",
                ["level"] = "Уровень",
                ["moves"] = "Количество ходов",
                ["level is completed"] = "Уровень пройден!",
                ["no levels found"] = "Информация об уровнях не найдена!",
                ["title"] = "Sokoban",
                ["play"] = "Играть",
                ["rules"] = "Правила игры",
                ["exit"] = "Выйти",
                ["language"] = "Русский",
                ["enterNickname"] = "Введите свой никнейм:",
                ["next"] = "Далее",
                ["nicknamePlaceholder"] = "Введите никнейм...",
                ["rulesText"] = @"Сокобан - игра-головоломка.
Цель игры:
- Перемещайте ящики на целевые клетки
- Игрок может толкать только один ящик за раз
- Нельзя тянуть ящики или перепрыгивать через них
Управление:
- Стрелки или WASD для движения
Удачи!",
                ["rulesTitle"] = "Правила игры",
                ["selectLevel"] = "Выбрать уровень",
                ["history"] = "История",
                ["place"] = "Место",
                ["nickname"] = "Имя",
                ["rating"] = "Рейтинг",
                ["settings"] = "Настройки",
                ["resetLevel"] = "Перезапустить",
                ["playAgain"] = "Играть снова",
                ["nextLevel"] = "Следующий уровень",
                ["levelName"] = "Название уровня",
                ["completionTime"] = "Время прохождения",
                ["steps"] = "Количество шагов",
                ["completionDate"] = "Дата прохождения",
                ["levelsCompleted"] = "Пройдено уровней",
                ["back"] = "Назад"
            },
            [Language.Japanese] = new Dictionary<string, string>
            {
                ["time"] = "じかん",
                ["youAreInPlace"] = "{0} 位です :)",
                ["back to levels"] = "レベルにもどる",
                ["level"] = "レベル",
                ["moves"] = "うごかしたかいすう",
                ["level is completed"] = "レベルクリア！",
                ["no levels found"] = "レベルのじょうほうがみつかりません！",
                ["title"] = "ソコバン",
                ["play"] = "プレイ",
                ["rules"] = "ルール",
                ["exit"] = "しゅうりょう",
                ["language"] = "にほんご",
                ["enterNickname"] = "ニックネームをにゅうりょくしてください:",
                ["next"] = "つぎへ",
                ["nicknamePlaceholder"] = "ニックネームをにゅうりょく...",
                ["rulesText"] = @"ソコバンはパズルゲームです。
もくてき:
- はこをゴールにうごかす
- プレイヤーは1どにひとつのはこしかおせません
- はこをひっぱったり、とびこえたりできません
そうさほうほう:
- やじるしキーか WASD でうごく
がんばってください！",
                ["rulesTitle"] = "ルール",
                ["selectLevel"] = "レベルをせんたく",
                ["history"] = "りれき",
                ["rating"] = "ランキング",
                ["settings"] = "せってい",
                ["resetLevel"] = "レベルをさいきどう",
                ["playAgain"] = "もういちどプレイ",
                ["nextLevel"] = "つぎのレベル",
                ["levelName"] = "れべるめい",
                ["completionTime"] = "かんりょうじかん",
                ["steps"] = "すてっぷすう",
                ["completionDate"] = "かんりょうび",
                ["levelsCompleted"] = "かんりょうれべるすう",
                ["back"] = "もどる",
                ["place"] = "じゅんい",
                ["nickname"] = "ニックネーム",
            }
        };
    }
}