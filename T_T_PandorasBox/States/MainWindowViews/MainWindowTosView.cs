using ImGuiNET;

namespace T_T_PandorasBox.States.MainWindowViews;

public class MainWindowTosView : IMainWindowView
{
    public string Name => "TOS & EULA";
    
    public void Render(float deltaTime)
    {
        var wrapWidth = ImGui.GetContentRegionAvail().X;
        ImGui.PushTextWrapPos(ImGui.GetCursorPosX() + wrapWidth);
        
        ImGui.Text("1. ACCEPTANCE OF TERMS");
        ImGui.Indent(20.0f);
        ImGui.Text("\tBy using T_T Pandora's box, you agree to the following terms and conditions. If you do not agree with these Terms, please do not use this Software.");
        ImGui.Unindent(20.0f);
        ImGui.Spacing();
        ImGui.Spacing();
        
        ImGui.Text("2. USAGE OF THE SOFTWARE");
        ImGui.Indent(20.0f);
        ImGui.Text("\tYou are granted a limited, non-exclusive, non-transferable license to use the Software in accordance with these Terms and any other documentation provided.");
        ImGui.Unindent(20.0f);
        ImGui.Spacing();
        ImGui.Spacing();
        
        ImGui.Text("3. RESPONSIBILITY");
        ImGui.Indent(20.0f);
        ImGui.Text("\tYou assume full responsibility and risk of loss resulting from your use of the Software. You agree to use the Software responsibly and within the bounds of all applicable laws. ");
        ImGui.Text("\tIt is solely your responsibility to ensure that your use of the Software does not contradict with applicable laws and regulations, and koelion takes no responsibility for misuse.");
        ImGui.Unindent(20.0f);
        ImGui.Spacing();
        ImGui.Spacing();
        
        ImGui.Text("4. UNINTENDED USE");
        ImGui.Indent(20.0f);
        ImGui.Text("\tYou agree to indemnify and hold harmless koelion from any damages, losses, liabilities, judgments, or settlements, including reasonable attorney's fees, costs, and other expenses incurred by koelion as a result of your use of the Software in ways not intended by koelion.");
        ImGui.Unindent(20.0f);
        ImGui.Spacing();
        ImGui.Spacing();
        
        ImGui.Text("5. DISCLAIMER OF WARRANTIES");
        ImGui.Indent(20.0f);
        ImGui.Text("\tThe Software is provided \"AS IS\" without any express or implied warranties of any kind. koelion makes no representations or guarantees of the Software's functionality, reliability, or suitability for any particular purpose.");
        ImGui.Unindent(20.0f);
        ImGui.Spacing();
        ImGui.Spacing();
        
        ImGui.Text("6. LIMITATION OF LIABILITY");
        ImGui.Indent(20.0f);
        ImGui.Text("\tIn no event shall koelion be liable for any direct, indirect, incidental, special, exemplary, or consequential damages arising from or related to your use or inability to use the Software.");
        ImGui.Unindent(20.0f);
        ImGui.Spacing();
        ImGui.Spacing();
        
        ImGui.Text("7. EDUCATIONAL PURPOSES AND COMPETITIVE USE");
        ImGui.Indent(20.0f);
        ImGui.Text("\tThe Software is designed and provided for educational purposes only. The information and tools provided by the Software are not intended to give users an unfair advantage in any competitive environment, including but not limited to video games, sports, or any other competitions.");
        ImGui.Text("\tUsers are strictly prohibited from using the Software in any competitive games or events, whether formal or informal. Any such use is in violation of these Terms and may also violate the rules or terms of the competitive game or event in question.");
        ImGui.Text("\tBy using the Software, you acknowledge and agree that you are using it solely for educational and non-competitive purposes. If you use the Software in violation of this clause, you do so at your own risk and assume all legal and other consequences.");
        ImGui.Text("\tAny use of the Software in a manner inconsistent with its intended educational purpose, as determined by koelion, will be grounds for the immediate termination of your license to use the Software.");
        ImGui.Unindent(20.0f);
        ImGui.Spacing();
        ImGui.Spacing();
        
        ImGui.Text("8. USER-MADE SCRIPTS AND THIRD-PARTY CONTENT");
        ImGui.Indent(20.0f);
        ImGui.Text("\tSoftware allows for the integration and use of user-made scripts and third-party content (\"User Content\"). By using the Software, you acknowledge and agree that you are solely responsible for any User Content you integrate or use in conjunction with the Software.");
        ImGui.Text("\tkoelion does not review, endorse, or take responsibility for any User Content, including its legality, reliability, accuracy, or appropriateness. We make no representations or warranties about the safety, functionality, or appropriateness of any User Content.");
        ImGui.Text("\tYou agree to indemnify and hold harmless koelion, its affiliates, officers, directors, employees, and agents from and against any claims, actions, demands, damages, losses, or expenses, including legal and professional fees, arising out of or in connection with your use of any User Content with the Software.");
        ImGui.Text("\tkoelion is not liable for any harm, damages, or losses resulting from your use of User Content, and you agree to use such content at your own risk.");
        ImGui.Text("\tIf you choose to use or integrate User Content, you must ensure that you have the necessary rights and permissions to do so and that your use does not infringe on the rights of any third parties.");
        ImGui.Text("\tkoelion reserve the right, at his sole discretion, to disable or remove any User Content that we believe violates these Terms or may harm the Software or its users.");
        ImGui.Unindent(20.0f);
        
        
        ImGui.PopTextWrapPos();
    }
}