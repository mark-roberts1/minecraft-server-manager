import React, { useState, useEffect } from 'react';
import { ServerInfo, ServerStatus } from '../models/ServerInfo';
import api from '../Controller';
import './ServerList.scss';
import './AddModal.scss';
import runningImg from '../media/server-running.png';
import stoppedImg from '../media/server-stopped.png';
import { Link } from 'react-router-dom';
import { CreateServerRequest } from '../models/CreateServerRequest';

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
            <div className="list">
                <div className="action-container">
                    <button className="server-add-btn" onClick={e => setServers({...serversState, showAdd: true})}>+ Add</button>
                </div>
                {serversState.servers.length == 0 &&
                    <div className="empty">
                        <h2>Nothing's going on here yet :(</h2>
                    </div>
                }
                {serversState.servers.length > 0 &&
                    serversState.servers.map((server, index) => {
                        return (
                            <div id={server.serverId.toString()} key={server.serverId} className="server">
                                {server.status == ServerStatus.Stopped &&
                                    <img className="server-field status-img" src={stoppedImg} />
                                }
                                {server.status == ServerStatus.Started &&
                                    <img className="server-field status-img" src={runningImg} />
                                }
                                <div className="server-field">
                                    <span className="server-name">{server.name}</span>
                                </div>
                                <div className="server-field small">
                                    <span className="server-version">{server.version}</span>
                                </div>
                                <div className="server-field large">
                                    <span className="server-description">{server.description}</span>
                                </div>
                            </div>
                        )
                    })
                }
            </div>
            {serversState.showAdd &&
                <div className="add-modal-backdrop">
                    <div className="add-modal">
                        <div className="modal-header">
                            <h2 className="header-item">New Server</h2>
                            <div className="close header-item" onClick={e => setServers({...serversState, showAdd: false})}>X</div>
                        </div>
                        <div className="modal-body">
                            <div className="field-wrapper">
                                <input type="textbox" className="field" placeholder="Name" name="name" 
                                    value={newServer.name} onChange={e => setNewServer({...newServer, name: e.target.value})} required />
                                <label htmlFor="name" className="field-label">Name</label>
                            </div>
                            <div className="field-wrapper">
                                <input type="textbox" className="field" placeholder="Version" name="version" 
                                    value={newServer.version} onChange={e => setNewServer({...newServer, version: e.target.value})} required />
                                <label htmlFor="version" className="field-label">Version</label>
                            </div>
                            <div className="field-wrapper full">
                                <input type="textbox" className="field" placeholder="Description" name="description" 
                                    value={newServer.description} onChange={e => setNewServer({...newServer, description: e.target.value})} required />
                                <label htmlFor="description" className="field-label">Description</label>
                            </div>
                            <ul className="server-properties">
                                {newServer.properties.map((property, index) => {
                                    return (
                                        <div className="field-wrapper">
                                            <input type="textbox" className="field" placeholder={property.key} name={property.key.replace(" ", "") + index} 
                                                value={property.value} onChange={e => handlePropUpdate(e, index)} required />
                                            <label htmlFor={property.key.replace(" ", "") + index} className="field-label">{property.key}</label>
                                        </div>
                                    )
                                })}
                            </ul>
                        </div>
                        <div className="modal-footer">
                            <button className="submit-btn modal-submit" onClick={e => addServer(e)}>Add</button>
                        </div>
                    </div>
                </div>
            }
        </div>
    )
}

export default ServerList;