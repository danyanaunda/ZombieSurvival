using System.Collections.Generic;
using UnityEngine;

public class ShellPool
{
    private Shell _shell;
    private Queue<Shell> _physicalAmmoPool;


    public ShellPool(Shell shell, int ammoCount)
    {
        _physicalAmmoPool = new Queue<Shell>(ammoCount);
        _shell = shell;
    }

    public Shell Get()
    {
        if (_physicalAmmoPool.TryDequeue(out var shell))
        {
            shell.gameObject.SetActive(true);
            return shell;
        }

        return CreateShell();
    }

    private Shell CreateShell()
    {
        var shell = Object.Instantiate(_shell);
        shell.OnHitEvent += Release;

        return shell;
    }

    public void Release(Shell shell)
    {
        shell.Deactivate();
        shell.gameObject.SetActive(false);
        _physicalAmmoPool.Enqueue(shell);
    }
}