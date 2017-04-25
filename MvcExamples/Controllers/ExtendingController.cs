using MvcExamples.Models;
using SeedPacket.Extensions;
using System.Collections.Generic;
using System.Web.Mvc;
using SeedPacket.Generators;
using SeedPacket;

namespace MvcExamples.Controllers
{
    public class ExtendingController : Controller
    {
        public ActionResult Index(int rows = 20, bool fromFile = false)
        {
            var fileGenerator = new MultiGenerator(sourceFilepath: "~/SourceFiles/sourceData2.xml");
            var sourceGenerator = new MultiGenerator(sourceString: GetXmlString());
            var generator = fromFile ? fileGenerator : sourceGenerator;

            var users = new List<User>().Seed(1, rows , generator);

            return View("Index", users);
        }

        private string GetXmlString() {

            return @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                            <Root>
                                <FirstNames>
                                    <FirstName>Will</FirstName>
                                    <FirstName>Patricia</FirstName>
                                    <FirstName>David</FirstName>
                                    <FirstName>Jennifer</FirstName>
                                    <FirstName>John</FirstName>
                                    <FirstName>Joe</FirstName>
                                    <FirstName>Xavier</FirstName>
                                    <FirstName>Rupneet</FirstName>
                                    <FirstName>Dianne</FirstName>
                                </FirstNames>
                                <LastNames>
                                    <LastName>Crowther</LastName>
                                    <LastName>Webb</LastName>
                                    <LastName>Smith</LastName>
                                    <LastName>Jones</LastName>
                                    <FirstName>Frank</FirstName>
                                    <FirstName>Scott</FirstName>
                                    <FirstName>Chelsea</FirstName>
                                    <FirstName>Kennedy</FirstName>
                                    <FirstName>Joyce</FirstName>
                                </LastNames>
                                  <CompanyNames>
                                    <CompanyName>Acme</CompanyName>
                                    <CompanyName>Rand</CompanyName>
                                    <CompanyName>Sears</CompanyName>
                                  </CompanyNames>
                                  <CompanySuffixes>
                                    <CompanySuffix></CompanySuffix>
                                    <CompanySuffix>Co</CompanySuffix>
                                    <CompanySuffix>Corp</CompanySuffix>
                                  </CompanySuffixes>
                                  <DomainExtensions>
                                    <DomainExtension>.com</DomainExtension>
                                    <DomainExtension>.com</DomainExtension>
                                    <DomainExtension>.net</DomainExtension>
                                  </DomainExtensions>
                            </Root>";
       }

    }
}