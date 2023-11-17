class Query {
    constructor(queryId, filterIds)
    {
        this.queryLink = document.getElementById(queryId);
        this.filterIds = JSON.parse(filterIds);
        console.log(filterIds);
    }

    updateFilterIds(input, filterId, categoryName)
    {
        const checked = input.checked;
        if (checked)
        {
            this.filterIds.push(filterId);
        }
        else
        {
            let index = this.filterIds.indexOf(filterId);
            if (index != -1)
            {
                this.filterIds.splice(index, 1);
            }
        }
        this.updateQuery(categoryName);
    }

    updateQuery(categoryName)
    {
        this.query = `/${categoryName}?filters=`;
        if (this.filterIds.length == 0) {
            this.query = `/${categoryName}`;
        }
        else
        {
            this.filterIds.forEach(filterId =>
            {
                this.query += `${filterId}|`
            });
        }
        this.queryLink.href = this.query
    }

}

