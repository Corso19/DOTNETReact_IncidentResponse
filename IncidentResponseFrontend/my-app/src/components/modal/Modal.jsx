import { Button } from "react-bootstrap";
import Modal from "react-modal";
import React from "react";

const FormModal = ({showModal, setShowModal, title, children}) => {
    return (
        <Modal isOpen={showModal}>
            <div className="modal-content d-flex card">
                {/* MODAL HEADER*/}
                <div className="modal-header align-items-center" style={{ width: "100%" }}>
                    <h1 className="text-center mx-auto py-0 my-0">{title}</h1>
                </div>
                <hr></hr>

                {/* MODAL BODY*/}
                <div className="modal-body"> 
                    {children}
                </div>
                <hr></hr>
                {/* Modal Footer */}
                <div className="modal-footer d-flex justify-content-between mb-0">
                    {/* <div className="d-flex align-items-center">
                    <LoaderButton
                        size={buttonsSize}
                        disabled={submitButtonDisabled}
                        loading={submitButtonLoading}
                        onClick={onSubmitClick}
                        text={submitBttnText ?? "Salvează"}
                        className="ms-3 px-4 me-1"
                        variant="outline-primary"
                        style={{ width: submitBttnSize ? submitBttnSize : "120px" }}
                    />

                    {submitButtonDisabled && disabledButtonTooltip && (
                        <Tooltip
                        title={
                            <span style={{ fontSize: "1.2em" }}>
                            {" "}
                            {disabledButtonTooltip}{" "}
                            </span>
                        }
                        placement="right"
                        >
                        <div>
                            <RiErrorWarningFill
                            style={{ color: "#DC3545", fontSize: "1.2em" }}
                            />
                        </div>
                        </Tooltip>
                    )}
                    </div> */}
                    <Button
                        className="me-3 ps-4 pe-4"
                        variant="outline-secondary"
                        onClick={() => setShowModal(false)}
                    >
                        Renunță
                    </Button>
                </div>
            </div>
        </Modal>
    );
}

export default FormModal;