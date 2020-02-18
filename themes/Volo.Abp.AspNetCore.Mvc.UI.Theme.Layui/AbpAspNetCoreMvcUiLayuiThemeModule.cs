﻿using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Layui.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Layui.Localization.Layui;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Layui.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Layui
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule)
        )]
    public class AbpAspNetCoreMvcUiLayuiThemeModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpThemingOptions>(options =>
            {
                options.Themes.Add<LayuiTheme>();

                if (options.DefaultThemeName == null)
                {
                    options.DefaultThemeName = LayuiTheme.Name;
                }
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<LayuiResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Layui");
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiLayuiThemeModule>("Volo.Abp.AspNetCore.Mvc.UI.Theme.Layui");
            });

            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new LayuiThemeMainTopToolbarContributor());
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add(LayuiThemeBundles.Styles.Global, bundle =>
                    {
                        bundle
                            .AddBaseBundles(StandardBundles.Styles.Global)
                            .AddContributors(typeof(LayuiThemeGlobalStyleContributor));
                    })
                    .Add(LayuiThemeBundles.Styles.Empty, bundle => bundle.AddContributors(typeof(LayuiThemeEmptyStyleContributor)))
                    .Add(LayuiThemeBundles.Styles.InputTags, bundle => bundle.AddFiles("/css/inputTags/inputTags.css"))
                    .Add(LayuiThemeBundles.Styles.SoulTable, bundle => bundle.AddFiles(
                        "/css/soulTable/animate.min.css", "/css/soulTable/soulTable.css"
                    ));

                options
                    .ScriptBundles
                    .Add(LayuiThemeBundles.Scripts.Global, bundle =>
                    {
                        bundle
                            .AddBaseBundles(StandardBundles.Scripts.Global)
                            .AddContributors(typeof(LayuiThemeGlobalScriptContributor));
                    })
                    .Add(LayuiThemeBundles.Scripts.Empty, bundle => bundle.AddContributors(typeof(LayuiThemeEmptyScriptContributor)))
                    .Add(LayuiThemeBundles.Scripts.SoulTable, bundle => bundle.AddFiles(
                        "/js/ext/excel.js", "/js/ext/xlsx.js",
                        "/js/ext/FileSaver.js", "/js/ext/tableChild.js",
                        "/js/ext/tableFilter.js", "/js/ext/soulTable.js"
                    ));
            });
        }
    }
}
