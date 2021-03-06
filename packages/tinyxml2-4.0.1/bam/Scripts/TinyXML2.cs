#region License
// Copyright (c) 2010-2019, Mark Final
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
//
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
//
// * Neither the name of BuildAMation nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion // License
using Bam.Core;
namespace TinyXML2
{
    [Bam.Core.ModuleGroup("Thirdparty/TinyXML2")]
    class TinyXML2Static :
        C.StaticLibrary
    {
        protected override void
        Init()
        {
            base.Init();

            this.CreateHeaderCollection("$(packagedir)/*.h");
            var source = this.CreateCxxSourceCollection("$(packagedir)/tinyxml2.cpp");
            source.PrivatePatch(settings =>
                {
                    var cxxCompiler = settings as C.ICxxOnlyCompilerSettings;
                    cxxCompiler.ExceptionHandler = C.Cxx.EExceptionHandler.Asynchronous;
                    cxxCompiler.LanguageStandard = C.Cxx.ELanguageStandard.Cxx11;

                    if (settings is VisualCCommon.ICommonCompilerSettings visualCCompiler)
                    {
                        visualCCompiler.WarningLevel = VisualCCommon.EWarningLevel.Level4;
                    }

                    if (settings is MingwCommon.ICommonCompilerSettings mingwCompiler)
                    {
                        mingwCompiler.AllWarnings = true;
                        mingwCompiler.ExtraWarnings = true;
                        mingwCompiler.Pedantic = true;
                    }

                    if (settings is GccCommon.ICommonCompilerSettings gccCompiler)
                    {
                        gccCompiler.AllWarnings = true;
                        gccCompiler.ExtraWarnings = true;
                        gccCompiler.Pedantic = true;

                        var gccMetaData = Bam.Core.Graph.Instance.PackageMetaData<Gcc.MetaData>("Gcc");
                        if (null != gccMetaData)
                        {
                            if (gccMetaData.ToolchainVersion.AtLeast(GccCommon.ToolchainVersion.GCC_7))
                            {
                                var compiler = settings as C.ICommonCompilerSettings;
                                compiler.DisableWarnings.AddUnique("implicit-fallthrough");
                            }
                        }
                    }

                    if (settings is ClangCommon.ICommonCompilerSettings clangCompiler)
                    {
                        clangCompiler.AllWarnings = true;
                        clangCompiler.ExtraWarnings = true;
                        clangCompiler.Pedantic = true;
                    }
                });

            this.PublicPatch((settings, appliedTo) =>
                {
                    if (settings is C.ICommonPreprocessorSettings preprocessor)
                    {
                        preprocessor.IncludePaths.AddUnique(this.CreateTokenizedString("$(packagedir)"));
                    }
                });
        }
    }

    [Bam.Core.ModuleGroup("Thirdparty/TinyXML2")]
    class TinyXML2Dynamic :
        C.Cxx.DynamicLibrary
    {
        protected override void
        Init()
        {
            base.Init();

            this.SetSemanticVersion(4, 0, 1);

            this.CreateHeaderCollection("$(packagedir)/*.h");
            var source = this.CreateCxxSourceCollection("$(packagedir)/tinyxml2.cpp");
            source.PrivatePatch(settings =>
                {
                    var preprocessor = settings as C.ICommonPreprocessorSettings;
                    var cxxCompiler = settings as C.ICxxOnlyCompilerSettings;
                    cxxCompiler.ExceptionHandler = C.Cxx.EExceptionHandler.Asynchronous;
                    cxxCompiler.LanguageStandard = C.Cxx.ELanguageStandard.Cxx11;

                    if (settings is VisualCCommon.ICommonCompilerSettings visualCCompiler)
                    {
                        visualCCompiler.WarningLevel = VisualCCommon.EWarningLevel.Level4;
                        preprocessor.PreprocessorDefines.Add("TINYXML2_EXPORT");
                    }

                    if (settings is MingwCommon.ICommonCompilerSettings mingwCompiler)
                    {
                        mingwCompiler.AllWarnings = true;
                        mingwCompiler.ExtraWarnings = true;
                        mingwCompiler.Pedantic = true;
                        preprocessor.PreprocessorDefines.Add("TINYXML2_EXPORT");
                    }

                    if (settings is GccCommon.ICommonCompilerSettings gccCompiler)
                    {
                        gccCompiler.AllWarnings = true;
                        gccCompiler.ExtraWarnings = true;
                        gccCompiler.Pedantic = true;

                        // brute force visibility
                        gccCompiler.Visibility = GccCommon.EVisibility.Default;

                        var gccMetaData = Bam.Core.Graph.Instance.PackageMetaData<Gcc.MetaData>("Gcc");
                        if (null != gccMetaData)
                        {
                            if (gccMetaData.ToolchainVersion.AtLeast(GccCommon.ToolchainVersion.GCC_7))
                            {
                                var compiler = settings as C.ICommonCompilerSettings;
                                compiler.DisableWarnings.AddUnique("implicit-fallthrough");
                            }
                        }
                    }

                    if (settings is ClangCommon.ICommonCompilerSettings clangCompiler)
                    {
                        clangCompiler.AllWarnings = true;
                        clangCompiler.ExtraWarnings = true;
                        clangCompiler.Pedantic = true;
                        // brute force visibility
                        clangCompiler.Visibility = ClangCommon.EVisibility.Default;
                    }
                });

            this.PublicPatch((settings, appliedTo) =>
                {
                    if (settings is C.ICommonPreprocessorSettings preprocessor)
                    {
                        preprocessor.IncludePaths.AddUnique(this.CreateTokenizedString("$(packagedir)"));
                        if (this.BuildEnvironment.Platform.Includes(Bam.Core.EPlatform.Windows) && appliedTo != this)
                        {
                            // applied everywhere it's not built
                            preprocessor.PreprocessorDefines.Add("TINYXML2_IMPORT");
                        }
                    }
                });
        }
    }
}
