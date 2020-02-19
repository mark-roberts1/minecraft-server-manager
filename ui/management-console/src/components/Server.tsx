import React, { useState, useEffect, useCallback } from 'react';
import { ServerInfo, ServerStatus } from '../models/ServerInfo';
import { useParams } from 'react-router-dom';
import api from '../Controller';
import runningImg from '../media/torch-on.png';
import stoppedImg from '../media/torch-off.png';
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import './Server.scss';
import { ServerProperty } from '../models/ServerProperty';
import { ServerCommandRequest } from '../models/ServerCommandRequest';
import Modal from 'react-bootstrap/Modal';
import { debounce } from 'lodash';
import { UpdateServerRequest } from '../models/UpdateServerRequest';

class ServerState {
    constructor() {
        this.didFetchInit = false;
        this.requestedDelete = false;
        this.server = new ServerInfo();
    }

    server: ServerInfo;
    didFetchInit: boolean;
    requestedDelete: boolean;
}

class CommandWindow {
    constructor() {
        this.outputLines = [];
        this.command = "";
    }

    outputLines: string[];
    command: string;
}

const Server: React.FC = () => {
    const [serverState, setServerState] = useState(new ServerState());
    const [commandWindowState, setCommandWindowState] = useState(new CommandWindow());

    let { serverId } = useParams();
    
    const updateServer = async (serverId: number, request: UpdateServerRequest) => {
        await api.updateServer(serverId, request);
    }

    const updateServerHandler = useCallback(debounce(updateServer, 400), []);

    const handlePropUpdate = (e: React.ChangeEvent<HTMLInputElement>, index: number) => {
        let items = [...serverState.server.properties];
        let prop = {...items[index]};
        prop.value = e.target.value;
        items[index] = prop;
        setServerState({...serverState, server: {...serverState.server, properties: items}});

        let request = new UpdateServerRequest();

        request.newName = serverState.server.name;
        request.description = serverState.server.description;
        request.newProperties = items;
        request.version = serverState.server.version;

        updateServerHandler(serverState.server.serverId, request);
    }

    const startServer = () => {
        api.startServer(serverState.server.serverId)
            .then(res => {
                setServerState({...serverState, server: {...serverState.server, status: ServerStatus.Started}});
            })
    }

    const stopServer = () => {
        api.stopServer(serverState.server.serverId)
            .then(res => {
                setServerState({...serverState, server: {...serverState.server, status: res ? ServerStatus.Stopped : ServerStatus.Started}});
            })
    }

    const handleSendCommand = () => {
        if (commandWindowState.command === "clear") {
            setCommandWindowState({
                command: "",
                outputLines: []
            });

            return;
        }

        let request = new ServerCommandRequest();
        request.command = commandWindowState.command;

        api.executeCommand(serverState.server.serverId, request)
            .then(res => {
                let lines = [...commandWindowState.outputLines];
                lines.push(res.log);

                setCommandWindowState({ command: "", outputLines: lines});
            })
    }

    useEffect(() => {
        if (!serverState.didFetchInit) {
            api.getServer(parseInt(serverId ?? "0"))
                .then(result => {
                    setServerState({...serverState, didFetchInit: true, server: result });
                });
        }
    })

    const hideDelete = () => {
        setServerState({...serverState, requestedDelete: false});
    }

    const onDelete = () => {
        api.deleteServer(serverState.server.serverId)
            .then(res => {
                if (res.serverDeleted) {
                    window.location.href = "/servers";
                }
            })
    }

    return (
    <div className="server">
        <Modal centered show={serverState.requestedDelete} onHide={hideDelete}>
            <Modal.Header closeButton>
                <Modal.Title>Delete Server</Modal.Title>
            </Modal.Header>

            <Modal.Body>
                <p>Are you sure? This is irreversible.</p>
            </Modal.Body>

            <Modal.Footer>
                <Button variant="secondary" onClick={hideDelete}>Cancel</Button>
                <Button variant="danger" onClick={onDelete}>Delete</Button>
            </Modal.Footer>
        </Modal>
        <div className="row pt-4">
                <h2 className="col-md-8 col-lg-8 col-sm-8">{serverState.server.name}</h2>
                <div className="col-md-4 col-lg-4 col-sm-4">
                    <div className="float-right  mr-4">
                        <img className="status-img" src={serverState.server.status == ServerStatus.Started ? runningImg : stoppedImg} />
                        <Button 
                            disabled={serverState.server.status == ServerStatus.Started} 
                            onClick={(e: any) => startServer()}
                            className="start-button ml-3">Start</Button>
                        <Button 
                            disabled={serverState.server.status == ServerStatus.Stopped}
                            onClick={(e: any) => stopServer()}
                            variant="light" 
                            className="stop-button ml-3">Stop</Button>
                    </div>
                </div>
        </div>
        <div className="row pt-2">
            <div className="col-md-6 col-lg-6 col-sm-6">
                <label className="">v{serverState.server.version}</label>
            </div>
            <div className="col-md-6 col-lg-6 col-sm-6">
                <Button variant="danger" 
                    className="delete-button float-right mr-4"
                    onClick={(e: any) => setServerState({...serverState, requestedDelete: true})}>Delete</Button>
            </div>
        </div>
        
        <div className="row pt-4">
            <div className="col-md-11 col-lg-11 col-sm-11"><p>{serverState.server.description}</p></div>
        </div>

        <div className="console-wrapper col-md-12 col-lg-12 col-sm-12 p-3">
            <pre className="console-output p-3">
                <samp>Server says:</samp><br/>
                {
                    commandWindowState.outputLines.map((line, index) => {
                        return (
                            <samp key={index}>{line}<br/></samp>
                        )
                    })
                }
            </pre>
            <div className="input-group console-input">
                <input type="text" 
                    className="form-control" 
                    placeholder="Enter Command" 
                    aria-label="Enter Command" 
                    value={commandWindowState.command} 
                    onChange={e => setCommandWindowState({...commandWindowState, command: e.target.value})}
                    onKeyUp={e => {
                        if (e.key === "Enter") {
                            handleSendCommand();
                        }
                    }} />
                <div className="input-group-append">
                    <Button variant="dark" onClick={(e: any) => handleSendCommand()}>Send</Button>
                </div>
            </div>
        </div>
        <div className="row pt-3">
            <h3 className="col-md-12 col-lg-12 col-sm-12">Server Properties:</h3>
        </div>
        <Table responsive striped hover variant="dark" className="pt-3">
            <thead>
                <tr>
                    <th>Key</th>
                    <th>Value</th>
                </tr>
            </thead>
            <tbody>
                {serverState.server.properties.map((property, index) => {
                    return (
                        <tr key={index}>
                            <td>{property.key}</td>
                            <td><input itemType="textbox" className="property-input col-md-12 col-lg-12 col-sm-12" value={property.value} onChange={e => handlePropUpdate(e, index)} /></td>
                        </tr>
                )})}
            </tbody>
        </Table>
    </div>
    )
}

export default Server;