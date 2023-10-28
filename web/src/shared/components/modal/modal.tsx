import { PropsWithChildren } from "react";
import { default as ReactModal } from "react-modal";
import { BLACK } from "shared/constants";

type ModalProps = {
  label: string;
  isOpen: boolean;
  onClose(): void;
};

export const Modal = ({
  label,
  isOpen,
  onClose,
  children,
  ...props
}: PropsWithChildren<ModalProps>) => {
  return (
    <ReactModal
      {...props}
      contentLabel={label}
      isOpen={isOpen}
      onRequestClose={onClose}
      shouldCloseOnEsc
      shouldCloseOnOverlayClick
      style={{
        content: {
          position: "absolute",
          top: "50%",
          left: "50%",
          transform: "translate(-50%, -50%)",
          height: "fit-content",
          width: "fit-content",
          padding: 24,
          zIndex: 2,
        },
        overlay: {
          backgroundColor: BLACK + "80", //hex code appended with alpha value 
          zIndex: 2,
        },
      }}
    >
      {children}
    </ReactModal>
  );
};

Modal.displayName = "Modal";
