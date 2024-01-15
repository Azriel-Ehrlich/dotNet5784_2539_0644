namespace Dal;
using DO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Data.Common;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    public int Create(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (engineers.Where(e => e.Id == item.Id).FirstOrDefault() is not null)
            throw new DalAlreadyExistsException($"Engineer with id {item.Id} already exists");
        engineers.Add(item);
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        Engineer? engineer = engineers.Where(eng=>eng.Id==id).FirstOrDefault() ?? throw new DalDoesNotExistException($"Engineer with id {id} doesn't exist");
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        if (tasks.Where(t => t.EngineerId == id && t.Active == true).FirstOrDefault() is not null)
            throw new DalDoesNotExistException($"Engineer with id {id} is still assigned to a task");
        Update(engineer with { Active = false });
                    
    }

    public Engineer? Read(int id)
    {
        return Read(x => x.Id == id);
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return ReadAll(filter).FirstOrDefault();
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (filter is null)
            return engineers.Select(e => e);
        return engineers.Where(filter);
    }

    public void Update(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        Engineer? engineer = Read(item.Id) ?? throw new DalDoesNotExistException($"Engineer with id {item.Id} doesn't exist");
        engineers.Remove(engineer);
        engineers.Add(item);
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);
    }
}
