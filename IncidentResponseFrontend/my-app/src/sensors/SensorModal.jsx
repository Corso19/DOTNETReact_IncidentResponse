import { Col, Form, Row } from 'react-bootstrap';
import React, { useEffect, useState } from "react";
import API_KEYS from '../constants/api-keys';
import FormModal from '../components/modal/Modal';

const SensorModal = ({sensor, showModal, setShowModal}) => {
    return (
        <FormModal
            showModal={showModal}
            setShowModal={setShowModal}
            title={sensor ? "Update sensor"  : "Add sensor"}
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
                        value={sensor?.name}
                    />
                </Col>
            </Form.Group>
        </FormModal>
    );
}
export default SensorModal;