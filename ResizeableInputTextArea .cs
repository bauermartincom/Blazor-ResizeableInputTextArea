public class ResizeableInputTextArea : InputBase<string?>
{
    /// <summary>
    /// Number of rows
    /// </summary>
    private int _rows;

    /// <summary>
    /// Max number of rows
    /// </summary>
    public int MaxRows { set; get; } = 50;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "textarea");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, "class", CssClass);
        builder.AddAttribute(3, "value", BindConverter.FormatValue(CurrentValue));
        builder.AddAttribute(4, "onchange", EventCallback.Factory.CreateBinder<string?>(this, __value =>
        {
            CurrentValueAsString = __value;
            //CalculateSize(CurrentValue);
        }, CurrentValueAsString));
        builder.AddAttribute(5, "rows", _rows);
        builder.CloseElement();
    }

    protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;
        return true;
    }

    protected override Task OnParametersSetAsync()
    {
        CalculateSize(CurrentValue);
        return base.OnParametersSetAsync();
    }

    protected void CalculateSize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            _rows = 2;
            return;
        }

        var rows = Math.Max(value.Split('\n').Length, value.Split('\r').Length);
        rows = Math.Max(rows, 2);
        _rows = Math.Min(rows, MaxRows);
    }
}
