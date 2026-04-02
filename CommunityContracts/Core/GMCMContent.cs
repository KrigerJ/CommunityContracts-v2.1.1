using StardewModdingAPI;
using GenericModConfigMenu;
using static ModEntry;

namespace CommunityContracts.Core
{
    internal class GMCMContent
    {
        public static void Register(IModHelper helper, IManifest manifest, IMonitor monitor, ModConfig config)
        {
            var gmcm = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm == null)
                return;

            gmcm.Register(
                mod: manifest,
                reset: () => config = new ModConfig(),
                save: () => helper.WriteConfig(config)
            );

            gmcm.AddSectionTitle(manifest, () => T("HotkeySettings"));

            gmcm.AddKeybind(
                mod: manifest,
                getValue: () => config.CheatMenuHotkey,
                setValue: value => config.CheatMenuHotkey = value,
                name: () => T("ShortcutMenuHotkey"),
                tooltip: () => T("ShortcutMenuHotkeyTooltip")
            );

            gmcm.AddSectionTitle(manifest, () => T("DeliveryChestSettings"));

            gmcm.AddTextOption(
                mod: manifest,
                getValue: () => config.DeliveryChestColor,
                setValue: value => config.DeliveryChestColor = value,
                name: () => T("ChestColor"),
                tooltip: () => T("ChestColorTooltip"),
                allowedValues: config.ChestColors.Keys
                    .Select(key => T("ChestColor" + key.Replace(" ", "")))
                    .ToArray()
            );

            gmcm.AddTextOption(
                mod: manifest,
                getValue: () => config.HighlightColor,
                setValue: value => config.HighlightColor = value,
                name: () => T("HighlightColor"),
                tooltip: () => T("HighlightColorTooltip"),
                allowedValues: config.HighlightColors.Keys
                    .Select(key => T("HighlightColor" + key.Replace(" ", "")))
                    .ToArray()
            );

            gmcm.AddTextOption(
                mod: manifest,
                getValue: () => config.FontColor,
                setValue: value => config.FontColor = value,
                name: () => T("FontColor"),
                tooltip: () => T("FontColorTooltip"),
                allowedValues: config.FontColors.Keys
                    .Select(key => T("FontColor" + key))
                    .ToArray()
            );

            gmcm.AddSectionTitle(manifest, () => T("ServiceSettings"));

            gmcm.AddBoolOption(
                mod: manifest,
                name: () => T("EnableMead"),
                tooltip: () => T("EnableMeadTooltip"),
                getValue: () => config.EnableMead,
                setValue: value => config.EnableMead = value
            );

            gmcm.AddBoolOption(
                mod: manifest,
                name: () => T("EnableProcessTimeReduction"),
                tooltip: () => T("EnableProcessTimeReductionTooltip"),
                getValue: () => config.EnableProcessTimeReduction,
                setValue: value => config.EnableProcessTimeReduction = value
            );

            gmcm.AddSectionTitle(manifest, () => T("CharacterSettings"));

            gmcm.AddBoolOption(
                mod: manifest,
                getValue: () => config.EnableContractTooltip,
                setValue: value => config.EnableContractTooltip = value,
                name: () => T("ShowContractTooltip"),
                tooltip: () => T("ShowContractTooltipTooltip")
            );

            gmcm.AddNumberOption(
                mod: manifest,
                getValue: () => config.CollectionDelay,
                setValue: value => config.CollectionDelay = value,
                name: () => T("CollectionDelay"),
                tooltip: () => T("CollectionDelayTooltip"),
                min: 100,
                max: 3000,
                interval: 100
            );

            gmcm.AddNumberOption(
                mod: manifest,
                getValue: () => config.NPCContractPercents["Basic"],
                setValue: value => config.NPCContractPercents["Basic"] = value,
                name: () => T("ContractPercentBasic"),
                tooltip: () => T("ContractPercentBasicTooltip"),
                min: 5,
                max: 95,
                interval: 5
            );

            gmcm.AddNumberOption(
                mod: manifest,
                getValue: () => config.NPCContractPercents["Custom"],
                setValue: value => config.NPCContractPercents["Custom"] = value,
                name: () => T("ContractPercentCustom"),
                tooltip: () => T("ContractPercentCustomTooltip"),
                min: 5,
                max: 95,
                interval: 5
            );
        }
    }
}
