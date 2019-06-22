/*
Copyright (c) 2010-2019, Mark Final
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.

* Neither the name of BuildAMation nor the names of its
  contributors may be used to endorse or promote products derived from
  this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#include "tinyxml2.h"

#include <iostream>

int main()
{
    tinyxml2::XMLDocument document;

    tinyxml2::XMLNode *root = document.NewElement("root");
    document.InsertFirstChild(root);

    tinyxml2::XMLElement *intEl = document.NewElement("IntValue");
    intEl->SetText(42);
    root->InsertEndChild(intEl);

    tinyxml2::XMLElement *floatEl = document.NewElement("FloatValue");
    floatEl->SetText(3.14f);
    root->InsertEndChild(floatEl);

    tinyxml2::XMLElement *dateEl = document.NewElement("DateValue");
    dateEl->SetAttribute("Day", 29);
    dateEl->SetAttribute("Month", "April");
    dateEl->SetAttribute("Year", 1976);
    root->InsertEndChild(dateEl);

    tinyxml2::XMLPrinter printer;
    document.Print(&printer);

    std::cout << printer.CStr() << std::endl;

    return 0;
}
