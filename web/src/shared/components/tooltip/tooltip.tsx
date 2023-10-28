import React, { PropsWithChildren, ReactElement } from "react";
import { Tooltip as ReactTooltip } from "react-tooltip";
import { BLACK } from "shared/constants";

type TooltipProps = {
  content: string | ReactElement;
  dataTooltipId: string;
};

export const Tooltip = ({
  content,
  dataTooltipId,
  children,
}: PropsWithChildren<TooltipProps>) => {
  return (
    <>
      {React.cloneElement(children as React.ReactElement<any>, {
        "data-tooltip-id": dataTooltipId,
      })}
      <ReactTooltip
        id={dataTooltipId}
        data-testid="tooltip-data-testid"
        style={{ backgroundColor: BLACK, width: '85%' }}
      >
        {content}
      </ReactTooltip>
    </>
  );
};

Tooltip.displayName = "Tooltip";
