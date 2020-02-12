import React, { useState, useEffect } from 'react';
import { ServerInfo, ServerStatus } from '../models/ServerInfo';
import { useParams } from 'react-router-dom';
import api from '../Controller';
import runningImg from '../media/server-running.png';
import stoppedImg from '../media/server-stopped.png';

const Server: React.FC = () => {
    const [serverState, setServerState] = useState(new ServerInfo());
    let { serverId } = useParams();
    let didLoad: boolean = false;

    const handlePropUpdate = (e: React.ChangeEvent<HTMLInputElement>, index: number) => {
        let items = [...serverState.properties];
        let prop = {...items[index]};
        prop.value = e.target.value;
        items[index] = prop;
        setServerState({...serverState, properties: items})
    }

    useEffect(() => {
        if (!didLoad) {
            api.getServer(parseInt(serverId ?? "0"))
                .then(result => {
                    setServerState(result);
                    didLoad = true;
                });
        }
    })

    return (
    <div className="server">
        {serverState.status == ServerStatus.Stopped &&
            <img className="server-field status-img" src={stoppedImg} />
        }
        {serverState.status == ServerStatus.Started &&
            <img className="server-field status-img" src={runningImg} />
        }
        <h2>{serverState.name}</h2>
        <label>{serverState.version}</label>
        <p>{serverState.description}</p>

        <button className="button start-button">Start</button>
        <button className="button stop-button">Stop</button>
        <button className="button delete-button">Delete</button>

        <input itemType="textbox" className="command-textbox" />
        
        <button className="button send-cmd-button">Send</button>

        <ul>
            {
                serverState.properties.map((property, index) => {
                    return (
                        <li key={index}>
                            <label>{property.key}</label>
                            <input itemType="textbox" value={property.value} onChange={e => handlePropUpdate(e, index)} />
                        </li>
                    )
                })
            }
        </ul>
    </div>
    )
}

export default Server;