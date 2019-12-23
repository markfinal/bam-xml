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
namespace GenerateXMLTest1
{
    abstract class GenerateXMLTest1_Common :
        C.Cxx.ConsoleApplication
    {
        protected C.Cxx.ObjectFileCollection Source;

        protected override void
        Init()
        {
            base.Init();

            this.Source = this.CreateCxxSourceCollection("$(packagedir)/source/*.cpp");
            this.Source.PrivatePatch(settings =>
            {
                var compiler = settings as C.ICommonCompilerSettings;
                compiler.WarningsAsErrors = true;

                if (settings is C.ICxxOnlyCompilerSettings cxxCompiler)
                {
                    cxxCompiler.ExceptionHandler = C.Cxx.EExceptionHandler.Asynchronous;
                }

                if (settings is VisualCCommon.ICommonCompilerSettings vcCompiler)
                {
                    vcCompiler.WarningLevel = VisualCCommon.EWarningLevel.Level4;
                }
                else if (settings is GccCommon.ICommonCompilerSettings gccCompiler)
                {
                    gccCompiler.AllWarnings = true;
                    gccCompiler.ExtraWarnings = true;
                    gccCompiler.Pedantic = true;
                }
                else if (settings is ClangCommon.ICommonCompilerSettings clangCompiler)
                {
                    clangCompiler.AllWarnings = true;
                    clangCompiler.ExtraWarnings = true;
                    clangCompiler.Pedantic = true;
                }
            });
        }
    }

    /*
    class GenerateXMLTest1_Static :
        GenerateXMLTest1_Common
    {
        protected override void
        Init()
        {
            base.Init();

            this.CompileAndLinkAgainst<TinyXML2.TinyXML2Static>(this.Source);
        }
    }
    */

    class GenerateXMLTest1_Dynamic :
        GenerateXMLTest1_Common
    {
        protected override void
        Init()
        {
            base.Init();

            this.UseSDK<TinyXML2.SDK>(this.Source);

            this.PrivatePatch(settings =>
            {
                if (settings is C.ICommonLinkerSettingsLinux linuxLinker)
                {
                    linuxLinker.CanUseOrigin = true;
                    linuxLinker.RPath.AddUnique("$ORIGIN");
                }
            });
        }
    }

    sealed class GenerateXMLTestRuntime :
        Publisher.Collation
    {
        protected override void
        Init()
        {
            base.Init();

            this.SetDefaultMacrosAndMappings(EPublishingType.ConsoleApplication);
            //this.Include<GenerateXMLTest1_Static>(C.Cxx.ConsoleApplication.ExecutableKey);
            this.Include<GenerateXMLTest1_Dynamic>(C.Cxx.ConsoleApplication.ExecutableKey);
        }
    }
}
