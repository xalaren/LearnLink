interface ITextContentProps {
    text: string;
}

function TextContent({ text }: ITextContentProps) {
    return (
        <div dangerouslySetInnerHTML={{ __html: text }} className="ui-text" />
    );
}

export default TextContent;