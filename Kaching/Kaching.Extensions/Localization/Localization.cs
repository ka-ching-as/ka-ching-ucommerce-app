﻿using System;
using System.Collections.Generic;
using System.Linq;
using Kaching.Extensions.Entities;
using Kaching.Extensions.Model;
using UCommerce.EntitiesV2;
using UCommerce.Infrastructure;

namespace Kaching.Extensions.Localization
{
    public interface IHasLocalizations
    {
        ICollection<ILocalized> Localizations { get; }
        string DefaultName { get; }
    }

    public interface ILocalized
    {
        string CultureCode { get; }
        string DisplayName { get; }
    }

    public class Localizer
    {
        private static string GetKachingLanguageCode(string cultureCode)
        {
            var languageCode = cultureCode.Split('-').First();
            var mappingRepository = ObjectFactory.Instance.Resolve<IRepository<KachingCultureCodeMapping>>();
            var cultureCodeMappings = mappingRepository.Select();

            var mapping = cultureCodeMappings.FirstOrDefault(m => m.CultureCode.ToLower() == cultureCode.ToLower());
            if (mapping != null)
            {
                return mapping.LanguageCode;
            }
            return languageCode;
        }

        public static L10nString GetLocalizedName<T>(T localized) where T : IHasLocalizations
        {
            var defaultName = new L10nString(HtmlUtilities.ConvertToPlainText(localized.DefaultName));
            if (localized.Localizations.Count == 0)
            {
                return defaultName;
            }
            else if (localized.Localizations.Count == 1)
            {
                foreach (var description in localized.Localizations)
                {
                    // There is only one

                    if (description.DisplayName == null || description.DisplayName.Length == 0)
                    {
                        return defaultName;
                    }

                    return new L10nString(HtmlUtilities.ConvertToPlainText(description.DisplayName));
                }
                // Will not actually happen. This is just to satisfy the compiler.
                return defaultName;
            }
            else
            {
                var localizations = new Dictionary<string, string>();
                foreach (var description in localized.Localizations)
                {
                    var languageCode = GetKachingLanguageCode(description.CultureCode);
                    if (description.DisplayName == null || description.DisplayName.Length == 0)
                    {
                        continue;
                    }
                    localizations[languageCode] = HtmlUtilities.ConvertToPlainText(description.DisplayName);
                }
                if (localizations.Count == 0)
                {
                    return defaultName;
                }
                return new L10nString(localizations);
            }
        }
    }
}
