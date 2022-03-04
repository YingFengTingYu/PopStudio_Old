using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopStudio.MAUI.Languages
{
    internal interface ILocalization
    {
        //Shell
        public string Shell_OK { get; }
        public string Shell_Cancel { get; }
        public string Shell_ExitTitle { get; }
        public string Shell_ExitText { get; }

        //Setting
        public string Setting_Title { get; }
        public string Setting_Introduction { get; }
        public string Setting_ItemLanguage { get; }
        public string Setting_SetLanguage { get; }
        public string Setting_ItemDz { get; }
        public string Setting_IntroDz { get; }
        public string Setting_Add { get; }
        public string Setting_Clear { get; }
        public string Setting_ItemPak { get; }
        public string Setting_IntroPak { get; }
        public string Setting_ItemRsb { get; }
        public string Setting_IntroRsb { get; }
        public string Setting_ItemPtx { get; }
        public string Setting_IntroPtx { get; }
        public string Setting_ItemCdat { get; }
        public string Setting_IntroCdat { get; }
        public string Setting_ItemRTON { get; }
        public string Setting_IntroRTON { get; }
        public string Setting_ItemCompiled { get; }
        public string Setting_IntroCompiled { get; }
        public string Setting_Load { get; }
        public string Setting_Unload { get; }
        public string Setting_Recover { get; }
        public string Setting_OK { get; }
        public string Setting_Cancel { get; }
        public string Setting_SureRecover { get; }
        public string Setting_SureRecoverText { get; }
        public string Setting_FinishRecover { get; }
        public string Setting_FinishRecoverText { get; }
        public string Setting_ChooseItem { get; }
        public string Setting_ChooseCompressMode { get; }
        public string Setting_EnterExtension { get; }
        public string Setting_EnterExtensionText { get; }
        public string Setting_CompressItem1 { get; }
        public string Setting_CompressItem2 { get; }
        public string Setting_ChooseLanguage { get; }
        public string Setting_FinishChooseLanguage { get; }
        public string Setting_FinishChooseLanguageText { get; }

        //Texture
        public string Texture_Title { get; }
        public string Texture_Introduction { get; }
        public string Texture_Mode1 { get; }
        public string Texture_Mode2 { get; }
        public string Texture_Choose1 { get; }
        public string Texture_Choose2 { get; }
        public string Texture_Choose3 { get; }
        public string Texture_Choose4 { get; }
        public string Texture_Choose5 { get; }
        public string Texture_Choose6 { get; }
        public string Texture_Choose7 { get; }

        //RTON
        public string RTON_Title { get; }
        public string RTON_Introduction { get; }
        public string RTON_Mode1 { get; }
        public string RTON_Mode2 { get; }
        public string RTON_Choose1 { get; }
        public string RTON_Choose2 { get; }
        public string RTON_Choose3 { get; }
        public string RTON_Choose4 { get; }
        public string RTON_Choose5 { get; }
        public string RTON_Choose6 { get; }

        //Trail
        public string Trail_Title { get; }
        public string Trail_Introduction { get; }
        public string Trail_Mode1 { get; }
        public string Trail_Mode2 { get; }
        public string Trail_Choose1 { get; }
        public string Trail_Choose2 { get; }
        public string Trail_Choose3 { get; }
        public string Trail_Choose4 { get; }
        public string Trail_Choose5 { get; }
        public string Trail_Choose6 { get; }

        //Reanim
        public string Reanim_Title { get; }
        public string Reanim_Introduction { get; }
        public string Reanim_Mode1 { get; }
        public string Reanim_Mode2 { get; }
        public string Reanim_Choose1 { get; }
        public string Reanim_Choose2 { get; }
        public string Reanim_Choose3 { get; }
        public string Reanim_Choose4 { get; }
        public string Reanim_Choose5 { get; }
        public string Reanim_Choose6 { get; }

        //Particles
        public string Particles_Title { get; }
        public string Particles_Introduction { get; }
        public string Particles_Mode1 { get; }
        public string Particles_Mode2 { get; }
        public string Particles_Choose1 { get; }
        public string Particles_Choose2 { get; }
        public string Particles_Choose3 { get; }
        public string Particles_Choose4 { get; }
        public string Particles_Choose5 { get; }
        public string Particles_Choose6 { get; }

        //Compress
        public string Compress_Title { get; }
        public string Compress_Introduction { get; }
        public string Compress_Mode1 { get; }
        public string Compress_Mode2 { get; }
        public string Compress_Choose1 { get; }
        public string Compress_Choose2 { get; }
        public string Compress_Choose3 { get; }
        public string Compress_Choose4 { get; }
        public string Compress_Choose5 { get; }
        public string Compress_Choose6 { get; }

        //Package
        public string Package_Title { get; }
        public string Package_Introduction { get; }
        public string Package_Mode1 { get; }
        public string Package_Mode2 { get; }
        public string Package_Choose1 { get; }
        public string Package_Choose2 { get; }
        public string Package_Choose3 { get; }
        public string Package_Choose4 { get; }
        public string Package_Choose5 { get; }
        public string Package_Choose6 { get; }
        public string Package_ChangeImage { get; }
        public string Package_DeleteImage { get; }
        //HomePage
        public string HomePage_Title { get; }
        public string HomePage_Begin { get; }
        public string HomePage_Function { get; }
        public string HomePage_Agreement { get; }
        public string HomePage_Version { get; }
        public string HomePage_Author_String { get; }
        public string HomePage_Author { get; }
        public string HomePage_Thanks_String { get; }
        public string HomePage_Thanks { get; }
        public string HomePage_QQGroup_String { get; }
        public string HomePage_QQGroup { get; }
        public string HomePage_Course_String { get; }
        public string HomePage_Course { get; }
        public string HomePage_AppNewNotice_String { get; }
        public string HomePage_AppNewNotice { get; }
        //Share
        public string Share_FileNotFound { get; }
        public string Share_FolderNotFound { get; }
        public string Share_Finish { get; }
        public string Share_Wrong { get; }
        public string Share_ChooseMode { get; }
        public string Share_Choose { get; }
        public string Share_Run { get; }
        public string Share_RunStatue { get; }
        public string Share_Waiting { get; }
        public string Share_Running { get; }

        //Permission
        public string Permission_Title { get; }
        public string Permission_Request1 { get; }
        public string Permission_Request2 { get; }
        public string Permission_GoTo { get; }
        public string Permission_Cancel { get; }

        //PickFile
        public string PickFile_AllFiles { get; }
        public string PickFile_NewFolder { get; }
        public string PickFile_Back { get; }
        public string PickFile_OK { get; }
        public string PickFile_Cancel { get; }
        public string PickFile_EnterFolderName { get; }
        public string PickFile_CreateWrong { get; }
        public string PickFile_NoPermission { get; }
        public string PickFile_NoPermissionToEnter { get; }
        public string PickFlie_SaveThere { get; }
        public string PickFlie_SaveFile { get; }
        public string PickFile_EnterFileName { get; }
        public string PickFile_ChooseThisFolder { get; }
    }
}
