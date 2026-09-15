import { validate } from "../../helpers/validation";

interface ISelectItemProps {
    title: string;
    onSelect: () => void;
}

function SelectItem({ title, onSelect }: ISelectItemProps) {
    let showItem = validate(title);
    return (
        <>
            {showItem &&
                <div className="select__item" onClick={onSelect} >
                    {title}
                </div>
            }
        </>
    );
}

export default SelectItem;