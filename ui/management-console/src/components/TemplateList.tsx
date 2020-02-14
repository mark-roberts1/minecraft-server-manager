import React, { useState, useEffect } from 'react';
import { Template } from '../models/Template';
import { AddTemplateRequest } from '../models/AddTemplateRequest';
import { ServerProperty } from '../models/ServerProperty';
import api from '../Controller';
import './TemplateList.scss';
import './AddModal.scss';
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import Modal from 'react-bootstrap/Modal';

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
            <Modal size="lg" centered show={templatesState.showAdd} onHide={() => setTemplates({...templatesState, showAdd: false})}>
                <Modal.Header closeButton>
                    <Modal.Title>Add Template</Modal.Title>
                </Modal.Header>

                <Modal.Body>
                    <div className="field-wrapper full">
                        <input type="textbox" className="field" placeholder="Name" name="name" 
                            value={newTemplate.name} onChange={e => setNewTemplate({...newTemplate, name: e.target.value})} required />
                        <label htmlFor="name" className="field-label">Name</label>
                    </div>
                    <div className="field-wrapper full">
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
                        <input type="textbox" className="field" placeholder="Download Link" name="downloadLink" 
                            value={newTemplate.downloadLink} onChange={e => setNewTemplate({...newTemplate, downloadLink: e.target.value})} required />
                        <label htmlFor="downloadLink" className="field-label">Download Link</label>
                    </div>
                </Modal.Body>

                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setTemplates({...templatesState, showAdd: false})}>Cancel</Button>
                    <Button variant="success" onClick={(e: any) => addTemplate(e)}>Create</Button>
                </Modal.Footer>
            </Modal>
            <div className="list-container">
                <div className="row action-container p-3">
                    <div className="filler col-md-10 col-sm-0 col-lg-10"></div>
                    <div className="col-md-2 col-sm-12 col-lg-2 float-right">
                        <Button variant="success" className="template-add-btn col-md-12 col-sm-12 col-lg-12" onClick={(e: any) => setTemplates({...templatesState, showAdd: true})}>+ Add</Button>
                    </div>
                </div>
                <Table responsive striped hover variant="dark" className="pt-3">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Version</th>
                            <th>Description</th>
                            <th>Download Link</th>
                        </tr>
                    </thead>
                    <tbody>
                        {templatesState.templates.map((template, index) => {
                            return (
                                <tr key={index}>
                                    <td>{template.name}</td>
                                    <td>{template.version}</td>
                                    <td>{template.description}</td>
                                    <td>{template.downloadLink}</td>
                                </tr>
                            )
                        })}
                    </tbody>
                </Table>
            </div>
        </div>
    )
}

export default TemplateList;