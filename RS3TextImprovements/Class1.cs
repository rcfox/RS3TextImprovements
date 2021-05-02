using HarmonyLib;
using UnityModManagerNet;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RS3TextImprovements
{
    static class Main
    {
        static void Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(MessageWindow))]
    [HarmonyPatch("ResettingMessSpeed")]
    static class MessageWindow_ResettingMessSpeed_Patch
    {
        static void Postfix(MessageWindow __instance)
        {
            __instance.message_speed = 2;
        }
    }

    [HarmonyPatch(typeof(CommandDescText))]
    [HarmonyPatch("DescTextUpdate")]
    static class CommandDescText_DescTextUpdate_Patch
    {
        static void Prefix(CommandDescText __instance)
        {
            GS.FontSize *= 0.5f;
            GS.m_font_scale_x *= 0.5f;
        }
        static void Postfix(CommandDescText __instance)
        {
            GS.FontSize *= 2.0f;
            GS.m_font_scale_x *= 2.0f;
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                // Adjust the text offset to be in the middle of the text window.
                if (instruction.Is(OpCodes.Ldc_I4, 475))
                {
                    yield return new CodeInstruction(OpCodes.Ldc_I4, 485);
                }
                else
                {
                    yield return instruction;
                }
            }
        }
    }
}