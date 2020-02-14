import React, { useState, useEffect } from 'react';
import { ServerInfo, ServerStatus } from '../models/ServerInfo';
import api from '../Controller';
import './ServerList.scss';
import './AddModal.scss';
import runningImg from '../media/server-running.png';
import stoppedImg from '../media/server-stopped.png';
import { Link, Route } from 'react-router-dom';
import { CreateServerRequest } from '../models/CreateServerRequest';
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import Modal from 'react-bootstrap/Modal';
import Td from './Td';

class ServersState {
    constructor(servers: ServerInfo[]) {
        this.servers = servers;
        this.loaded = false;
        this.showAdd = false;
    }

    showAdd: boolean;
    loaded: boolean;
    servers: ServerInfo[];
}

const ServerList: React.FC = () => {
    const [serversState, setServers] = useState(new ServersState([]));
    const [newServer, setNewServer] = useState(new CreateServerRequest());
    
    const handlePropUpdate = (e: React.ChangeEvent<HTMLInputElement>, index: number) => {
        let items = [...newServer.properties];
        let prop = {...items[index]};
        prop.value = e.target.value;
        items[index] = prop;
        setNewServer({...newServer, properties: items})
    }

    const addServer = (e: React.MouseEvent<HTMLButtonElement>) => {
        api.createServer(newServer)
            .then(result => {
                if (result.created) {
                    let items = serversState.servers;
        
                    items.push({
                        serverId: result.serverId,
                        name: newServer.name,
                        version: newServer.version,
                        description: newServer.description,
                        properties: newServer.properties,
                        status: ServerStatus.Stopped
                    });

                    setServers({...serversState, showAdd: false, servers: items });
                }
            })
    }

    useEffect(() => {
        if (!serversState.loaded) {
            api.listServers()
                .then(list => {
                    setServers({...serversState, servers: list, loaded: true});
                });
            
            api.getDefaultProperties()
                .then(props => {
                    setNewServer({...newServer, properties: props});
                })
        }
    });

    return (
        <div className="server-list">
            <Modal size="lg" centered show={serversState.showAdd} onHide={() => setServers({...serversState, showAdd: false})}>
                <Modal.Header closeButton>
                    <Modal.Title>Add Server</Modal.Title>
                </Modal.Header>

                <Modal.Body>
                    <div className="field-wrapper full">
                        <input type="textbox" className="field" placeholder="Name" name="name" 
                            value={newServer.name} onChange={e => setNewServer({...newServer, name: e.target.value})} required />
                        <label htmlFor="name" className="field-label">Name</label>
                    </div>
                    <div className="field-wrapper full">
                        <input type="textbox" className="field" placeholder="Version" name="version" 
                            value={newServer.version} onChange={e => setNewServer({...newServer, version: e.target.value})} required />
                        <label htmlFor="version" className="field-label">Version</label>
                    </div>
                    <div className="field-wrapper full">
                        <input type="textbox" className="field" placeholder="Description" name="description" 
                            value={newServer.description} onChange={e => setNewServer({...newServer, description: e.target.value})} required />
                        <label htmlFor="description" className="field-label">Description</label>
                    </div>
                    <Table responsive striped hover variant="dark" className="pt-3">
                        <thead>
                            <tr>
                                <th>Key</th>
                                <th>Value</th>
                            </tr>
                        </thead>
                        <tbody>
                            {newServer.properties.map((property, index) => {
                                return (
                                    <tr key={index}>
                                        <td>{property.key}</td>
                                        <td><input itemType="textbox" className="property-input col-md-12 col-lg-12 col-sm-12" value={property.value} onChange={e => handlePropUpdate(e, index)} /></td>
                                    </tr>
                            )})}
                        </tbody>
                    </Table>
                </Modal.Body>

                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setServers({...serversState, showAdd: false})}>Cancel</Button>
                    <Button variant="success" onClick={(e: any) => addServer(e)}>Create</Button>
                </Modal.Footer>
            </Modal>
            <div className="list-container">
                <div className="row action-container p-3">
                    <div className="filler col-md-10 col-sm-0 col-lg-10"></div>
                    <div className="col-md-2 col-sm-12 col-lg-2 float-right">
                        <Button variant="success" className="server-add-btn col-md-12 col-sm-12 col-lg-12" onClick={(e: any) => setServers({...serversState, showAdd: true})}>+ Add</Button>
                    </div>
                </div>
                <Table responsive striped hover variant="dark" className="pt-3">
                    <thead>
                        <tr>
                            <th>Status</th>
                            <th>Name</th>
                            <th>Version</th>
                            <th>Description</th>
                        </tr>
                    </thead>
                    <tbody>
                        {serversState.servers.map((server, index) => {
                            return (
                                <tr key={index}>
                                    <Td to={`server/${server.serverId}`}>
                                        <img className="server-field status-img" src={server.status == ServerStatus.Started ? runningImg : stoppedImg} />
                                    </Td>
                                    <Td to={`server/${server.serverId}`}>{server.name}</Td>
                                    <Td to={`server/${server.serverId}`}>{server.version}</Td>
                                    <Td to={`server/${server.serverId}`}>{server.description}</Td>
                                </tr>
                            )
                        })}
                    </tbody>
                </Table>
            </div>
        </div>
    )
}

export default ServerList;