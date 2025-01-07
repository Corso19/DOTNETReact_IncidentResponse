import { Col, Form, Row } from 'react-bootstrap';
import React, { useEffect, useState } from "react";
import FormModal from '../components/modal/Modal';
import { CrudService } from '../services/CrudService';
import ToggleButtonCustom from '../components/buttons/ToggleButtonCustom';

const AddUpdateSensorModal = ({sensor, setSensors, showModal, setShowModal}) => {
    const initialSensorObject = {
        name: "",
        isEnabled: false,
        clientSecret: "",
        tenantId: "",
        applicationId: ""
    };
    const submitButtonMessage = "All fields are required.";
    const [sensorObject, setSensorObject] = useState(initialSensorObject);
    const [submitButtonLoading, setSubmitButtonLoading] = useState(false);
    const submitButtonDisabled = !sensorObject.name || !sensorObject.clientSecret || 
                                 !sensorObject.tenantId || !sensorObject.applicationId;

    // initialize modal data
    useEffect(() => {
        if (sensor) {
            setSensorObject({
                name: sensor.sensorName,
                isEnabled: sensor.isEnabled,
                clientSecret: sensor.configuration.clientSecret,
                tenantId: sensor.configuration.tenantId,
                applicationId: sensor.configuration.applicationId,
              });
          } else {
            setSensorObject(initialSensorObject);
          }
    }, [sensor, showModal]);

    const handleInputChange = (field, value) => {
        console.log("Value: ", typeof value);
        // update the specific field of sensor object with the new value
        setSensorObject((previousSensor) => ({
          ...previousSensor,
          [field]: value
        }));
    };

    const onSubmit = () => {
        const newConfiguration = {
            clientSecret: sensorObject.clientSecret,
            tenantId: sensorObject.tenantId,
            applicationId: sensorObject.applicationId
        };
        const data = {
            sensorName: sensorObject.name,
            type: "MicrosoftEmail",
            isEnabled: sensorObject.isEnabled,
            configuration: JSON.stringify(newConfiguration)
        };
        setSubmitButtonLoading(true);
        try {
            if (sensor) {
              // Update existing sensor logic here
            } else {
                CrudService.create("Sensors", data).then((response) => {
                    if(response.status === 201){
                        // update local data
                        setSensors((previousSensors) => [...previousSensors, response.data]);
                    }
                    setSubmitButtonLoading(false);
                    console.log("Response: ", response);
                });
            }
            setShowModal(false);
        }
        catch(error){
            console.error("Error trying to add/update the sensor: ", error);
        }
    }

    return (
        <FormModal
            showModal={showModal}
            setShowModal={setShowModal}
            title={sensor ? "Update sensor"  : "Add sensor"}
            width="50%"
            onSubmit={onSubmit}
            disableSubmitButton={submitButtonDisabled}
            disabledMessage={submitButtonMessage}
            loadingSubmitButton={submitButtonLoading}
        >
            {/* Name field */}
            <Form.Group 
                as={Row} 
                controlId="sensorNameInput"
                className="mt-3"
            >
                <Form.Label column sm="4" className="ms-3">
                    Name:
                </Form.Label>
                <Col>
                    <Form.Control 
                        type="text" 
                        value={sensorObject["name"]} 
                        onChange={(event) => handleInputChange("name", event.target.value)} 
                    />
                </Col>
            </Form.Group>

            {/* Type field */}
            <Form.Group
                as={Row}
                controlId="sensorTypeInput"
                className="mt-3"
            >
                <Form.Label column sm="4" className="ms-3" >Type:</Form.Label>
                <Col>
                    <Form.Control 
                        type="text"
                        defaultValue="MicrosoftEmail"
                        disabled
                        readOnly
                    />
                </Col>
            </Form.Group>

            {/* Enabled field */}
            <Form.Group
                as={Row}
                controlId="sensorIsEnabledInput"
                className="mt-3"
            >
                <Form.Label column sm="4" className="ms-3" >Enabled:</Form.Label>
                <Col>
                    <ToggleButtonCustom
                        index={0}
                        value={sensorObject?.isEnabled ?? false}
                        setValue={(selectedValue) => handleInputChange("isEnabled", selectedValue)}
                    />
                </Col>
            </Form.Group>

            {/* Client secret field */}
            <Form.Group 
                as={Row} 
                controlId="clientSecretInput"
                className="mt-3"
            >
                <Form.Label column sm="4" className="ms-3">
                    Client Secret:
                </Form.Label>
                <Col>
                    <Form.Control 
                        type="text" 
                        value={sensorObject["clientSecret"]} 
                        onChange={(event) => handleInputChange("clientSecret", event.target.value)} 
                    />
                </Col>
            </Form.Group>

            {/* Tenant ID field */}
            <Form.Group 
                as={Row} 
                controlId="tenantIdInput"
                className="mt-3"
            >
                <Form.Label column sm="4" className="ms-3">
                    Tenant ID:
                </Form.Label>
                <Col>
                    <Form.Control 
                        type="text" 
                        value={sensorObject["tenantId"]} 
                        onChange={(event) => handleInputChange("tenantId", event.target.value)} 
                    />
                </Col>
            </Form.Group>

            {/* Application ID field */}
            <Form.Group 
                as={Row} 
                controlId="applicationIdInput"
                className="mt-3"
            >
                <Form.Label column sm="4" className="ms-3">
                    Application ID:
                </Form.Label>
                <Col>
                    <Form.Control 
                        type="text" 
                        value={sensorObject["applicationId"]} 
                        onChange={(event) => handleInputChange("applicationId", event.target.value)} 
                    />
                </Col>
            </Form.Group>
        </FormModal>
    );
}
export default AddUpdateSensorModal;