import React from "react";

interface ICollapseableBlockProps {
    title: string;
    collapsed: boolean;
    collapseChange: () => void;
    children: React.ReactNode;
}

function CollapseableBlock({ title, children, collapsed, collapseChange }: ICollapseableBlockProps) {
    return (
        <div className="collapseable-block">
            <div className="collapseable-block__header" onClick={collapseChange}>
                <p className="collapseable-block__title">{title}</p>
                <span className={`collapseable-block__icon icon icon-big-size  ${!collapsed ? 'icon-arrow-up' : 'icon-arrow-down'}`}></span>
            </div>
            <div
                className={`collapseable-block__collapse ${collapsed ? '' : 'collapseable-block__collapse-active'}`}
            >
                <div className="collapseable-block__body">
                    {children}
                </div>
            </div>
        </div >
    );
}

export default CollapseableBlock;