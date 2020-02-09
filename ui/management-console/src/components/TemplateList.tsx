import React, { useState, useEffect } from 'react';
import { Template } from '../models/Template';
import { AddTemplateRequest } from '../models/AddTemplateRequest';
import { ServerProperty } from '../models/ServerProperty';
import api from '../Controller';
import './TemplateList.scss';
import './AddModal.scss';

class TemplatesState {
    constructor(templates: Template[]) {
        this.templates = templates;
        this.loaded = false;
        this.showAdd = false;
    }

    showAdd: boolean;
    loaded: boolean;
    templates: Template[];
}

const TemplateList: React.FC = () => {
    const [templatesState, setTemplates] = useState(new TemplatesState([]));
    const [newTemplate, setNewTemplate] = useState(new AddTemplateRequest());
    const [defaultProperties, setDefaultProperties] = useState<ServerProperty[]>([]);

    const addTemplate = (e: React.MouseEvent<HTMLButtonElement>) => {
        api.addTemplate(newTemplate)
            .then(result => {
                if (result.templateAdded) {
                    let items = templatesState.templates;
        
                    items.push({
                        templateId: result.templateId,
                        name: newTemplate.name,
                        version: newTemplate.version,
                        description: newTemplate.description,
                        downloadLink: newTemplate.downloadLink,
                        properties: defaultProperties
                    });
            
                    setTemplates({...templatesState, showAdd: false, templates: items }); 
                }
            });
    }

    useEffect(() => {
        if (!templatesState.loaded) {
            api.listTemplates()
                .then(items => setTemplates({...templatesState, loaded: true, templates: items}));
            
            api.getDefaultProperties()
                .then(props => setDefaultProperties(props));
        }
    });

    return (
        <div className="template-list">
            <div className="list">
                <div className="action-container">
                    <button className="template-add-btn" onClick={e => setTemplates({...templatesState, showAdd: true})}>+ Add</button>
                </div>
                {templatesState.templates.length == 0 &&
                    <div className="empty">
                        <h2>Nothing's going on here yet :(</h2>
                    </div>
                }
                {templatesState.templates.length > 0 &&
                    templatesState.templates.map((template, index) => {
                        return (
                            <div id={template.templateId.toString()} className="template">
                                <div className="template-field">
                                    <span className="template-name">{template.name}</span>
                                </div>
                                <div className="template-field small">
                                    <span className="template-version">{template.version}</span>
                                </div>
                                <div className="template-field large">
                                    <span className="template-description">{template.description}</span>
                                </div>
                                <div className="template-field large">
                                    <span className="template-downloadlink">{template.downloadLink}</span>
                                </div>
                            </div>
                        )
                    })
                }
            </div>
            {templatesState.showAdd &&
                <div className="add-modal-backdrop">
                    <div className="add-modal">
                        <div className="modal-header">
                            <h2 className="header-item">New template</h2>
                            <div className="close header-item" onClick={e => setTemplates({...templatesState, showAdd: false})}>X</div>
                        </div>
                        <div className="modal-body">
                            <div className="field-wrapper">
                                <input type="textbox" className="field" placeholder="Name" name="name" 
                                    value={newTemplate.name} onChange={e => setNewTemplate({...newTemplate, name: e.target.value})} required />
                                <label htmlFor="name" className="field-label">Name</label>
                            </div>
                            <div className="field-wrapper">
                                <input type="textbox" className="field" placeholder="Version" name="version" 
                                    value={newTemplate.version} onChange={e => setNewTemplate({...newTemplate, version: e.target.value})} required />
                                <label htmlFor="version" className="field-label">Version</label>
                            </div>
                            <div className="field-wrapper full">
                                <input type="textbox" className="field" placeholder="Description" name="description" 
                                    value={newTemplate.description} onChange={e => setNewTemplate({...newTemplate, description: e.target.value})} required />
                                <label htmlFor="description" className="field-label">Description</label>
                            </div>
                            <div className="field-wrapper full">
                                <input type="textbox" className="field" placeholder="DownloadLink" name="DownloadLink" 
                                    value={newTemplate.downloadLink} onChange={e => setNewTemplate({...newTemplate, downloadLink: e.target.value})} required />
                                <label htmlFor="DownloadLink" className="field-label">Download Link</label>
                            </div>
                        </div>
                        <div className="modal-footer">
                            <button className="submit-btn modal-submit" onClick={e => addTemplate(e)}>Add</button>
                        </div>
                    </div>
                </div>
            }
        </div>
    )
}

export default TemplateList;