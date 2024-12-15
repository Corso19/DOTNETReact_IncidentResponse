import React, {useState} from "react";
import { PencilFill, TrashFill } from 'react-bootstrap-icons';
import IconButton from "../components/buttons/IconButton";

const SensorRow = ({sensor, setSensors}) => {
    const [deleteSensorLoading, setDeleteSensorLoading] = useState(false);
    const [showDeleteSensorModal, setShowDeleteSensorModal] = useState(false);
    const [showUpdateSensorModal, setShowUpdateSensorModal] = useState(false);
    return(
        <tr>
            <td align="left">
                {sensor.name}
            </td>
            <td>
                {sensor.type}
            </td>
            <td>
                {sensor.is_enabled ? "YES" : "NO"}
            </td>
            <td>
                <div className="d-flex align-items-center justify-content-evenly">
                    <PencilFill
                        title="Edit sensor"
                        size={18}
                        className="clickable color-primary-hover"
                        cursor="pointer"
                        onClick={() => {setShowUpdateSensorModal(true)}}
                    />
                    <IconButton
                        icon={TrashFill}
                        title="Delete sensor"
                        loading={deleteSensorLoading}
                        linkClassName="text-danger"
                        iconSize={18}
                        variant="danger"
                        onClick={() => setShowDeleteSensorModal(true)}
                    />
                </div>
            </td>
        </tr>
    );
}

export default SensorRow;