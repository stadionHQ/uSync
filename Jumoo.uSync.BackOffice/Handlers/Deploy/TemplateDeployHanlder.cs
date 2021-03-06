﻿using Jumoo.uSync.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Jumoo.uSync.BackOffice.Handlers.Deploy
{
    public class TemplateDeployHanlder : BaseDepoyHandler<IFileService, ITemplate>, ISyncHandler, IPickySyncHandler
    {
        IFileService _fileService;

        public TemplateDeployHanlder()
        {
            _fileService = ApplicationContext.Current.Services.FileService;
            _baseSerializer = uSyncCoreContext.Instance.TemplateSerializer;
            SyncFolder = Constants.Packaging.TemplateNodeName;
        }


        public string Name
        {
            get
            {
                return "Deploy:TemplateHandler";
            }
        }

        public int Priority
        {
            get
            {
                return uSyncConstants.Priority.Templates + 500;
            }
        }

        public override IEnumerable<ITemplate> GetAllExportItems()
        {
            return _fileService.GetTemplates();
        }

        public override ChangeType DeleteItem(uSyncDeployNode node, bool force)
        {
            var item = _fileService.GetTemplate(node.Key);
            if (item != null)
            {
                _fileService.DeleteTemplate(item.Alias);
                return ChangeType.Delete;
            }
            return ChangeType.NoChange;
        }

        public void RegisterEvents()
        {
            FileService.SavedTemplate += base.Service_Saved;
            FileService.DeletedTemplate += base.Service_Deleted;
        }
    }
}
