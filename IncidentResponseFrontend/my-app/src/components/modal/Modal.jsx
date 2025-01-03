import { Button } from "react-bootstrap";
import Modal from "react-modal";
import React from "react";
import LoaderButton from "../buttons/LoaderButton";
import { Tooltip } from "@mui/material";
import { RiErrorWarningFill } from "react-icons/ri";

const FormModal = ({
    showModal, 
    setShowModal, 
    title, 
    children, 
    submitButtonDisabled,
    disabledButtonMessage, 
    submitButtonLoading, 
    onSubmitClick,
    width
}) => {
    return (
        <Modal 
            isOpen={showModal}
            style={{
                content: {
                    top: "50%",
                    left: "50%",
                    right: "auto",
                    bottom: "auto",
                    transform: "translate(-50%, -50%)",
                    backgroundColor: "white",
                    width: width,
                    height: "auto",
                    maxHeight: "80vh",
                    maxWidth: "90%",
                    overflowY: "auto",
                    overflowX: "hidden"
                }
            }}
        >
            <div className="modal-content d-flex card">
                {/* MODAL HEADER*/}
                <div className="modal-header align-items-center color-bs-primary py-2" style={{ width: "100%"}}>
                    <h1 className="text-center mx-auto py-0 my-0">{title}</h1>
                </div>
                <hr className="custom-hr"></hr>

                {/* MODAL BODY*/}
                <div className="modal-body"> 
                    {children}
                </div>
                <hr className="custom-hr"></hr>

                {/* Modal Footer */}
                <div className="modal-footer d-flex justify-content-between mb-0 py-2">
                    <Button
                        className="ms-3 px-4"
                        variant="outline-secondary"
                        onClick={() => setShowModal(false)}
                    >
                        Cancel
                    </Button>
                    <div className="d-flex align-items-center">
                        <LoaderButton
                            disabled={submitButtonDisabled}
                            loading={submitButtonLoading}
                            onClick={onSubmitClick}
                            text="Save"
                            className="me-3 px-4"
                            variant="outline-primary"
                        />
                        {submitButtonDisabled && (
                            <Tooltip
                                title={
                                    <span style={{ fontSize: "1.2em" }}>
                                        {disabledButtonMessage}
                                    </span>
                                }
                                placement="right"
                            >
                                <div>
                                    <RiErrorWarningFill style={{ color: "#DC3545", fontSize: "1.2em" }}/>
                                </div>
                            </Tooltip>
                        )}
                    </div>
                </div>
            </div>
        </Modal>
    );
}

export default FormModal;