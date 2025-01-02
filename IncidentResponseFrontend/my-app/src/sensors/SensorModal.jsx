import { Col, Form, Row } from 'react-bootstrap';
import React, { useEffect, useState } from "react";
import API_KEYS from '../constants/api-keys';
import FormModal from '../components/modal/Modal';

const SensorModal = ({sensor, showModal, setShowModal}) => {
    const [name, setName] = useState("");
    const [type, setStype] = useState("");
    const [isEnabled, setIsEnabled] = useState(false);

    // initialize modal data
    useEffect(() => {
        if(sensor){
            setName(sensor.name);
            setStype(sensor.type);
            setIsEnabled(sensor.is_enabled);
        }
    }, [showModal, sensor]);

    // update sensor data
    const updateSensor = () => {}

    return (
        <FormModal
            showModal={showModal}
            setShowModal={setShowModal}
            title={sensor ? "Update sensor"  : "Add sensor"}
            width="50%"
        >
            <Form.Group
                as={Row}
                controlId="sensorNameInput"
                className="mt-3"
            >
                <Form.Label column sm="4" className="ms-3" >Name:</Form.Label>
                <Col>
                    <Form.Control 
                        type="text"
                        value={name}
                        onChange={(event) => setName(event.target.name)}
                    />
                </Col>
            </Form.Group>
            <Form.Group
                as={Row}
                controlId="sensorTypeInput"
                className="mt-3"
            >
                <Form.Label column sm="4" className="ms-3" >Type:</Form.Label>
                <Col>
                    <Form.Control 
                        type="text"
                        value={type}
                        onChange={(event) => setStype(event.target.value)}
                    />
                </Col>
            </Form.Group>
            <Form.Group
                as={Row}
                controlId="sensorIsEnabledInput"
                className="mt-3"
            >
                <Form.Label column sm="4" className="ms-3" >Enabled:</Form.Label>
                <Col>
                    <Form.Control 
                        type="text"
                        value={isEnabled}
                        onChange={(event) => setIsEnabled(event.target.value)}
                    />
                </Col>
            </Form.Group>
        </FormModal>
    );
}
export default SensorModal;